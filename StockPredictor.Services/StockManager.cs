using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using StockPredictorManagedWrapper;

namespace StockPredictor.Services
{
    public class StockManager
    {
        public IDataService DataService { get; set; }

        public StockManager()
        {
            
        }

        public double[] GetStockProjections(string symbol, int days, Action<string, StatusType> action)
        {
            var percentageComplete = 0;
            const int eachPart = 20;

            double rate1Year = Math.Round(DataService.GetYieldCurve()["1 YR"], 4);
            percentageComplete += eachPart;
            ReportStatus(action, string.Format("{0}% Finished Downloading yield curve..", percentageComplete), StatusType.Success);

            List<string> errors;
            var stock = DataService.GetStockQuote(new List<string> { symbol }, out errors).First();
            if (errors != null && errors.Contains(symbol))
            {
                var sb = new StringBuilder(string.Format("Unable to retrieve stock quote for {0}. ", symbol));
                if (DataService.IsValidateSymbol(symbol) == 0)
                {
                    sb.AppendLine(string.Format("Not a valid symbol. ", symbol));
                    var lookUp = DataService.LookUpStock(symbol);
                    if (lookUp == null)
                    {
                        sb.Append("No matching symbols found");
                    }
                    else
                    {
                        sb.Append("Did you mean?");
                        sb.AppendLine("");
                        foreach (var item in lookUp.Take(5))
                        {
                            sb.AppendLine(item);
                        }
                    }
                }
                ReportStatus(action, sb.ToString(), StatusType.Fail);
                return null;
            }

            percentageComplete += eachPart;
            ReportStatus(action, string.Format("{0}% Finished downloading stock quote..", percentageComplete), StatusType.Success);

            double? volatility = DataService.GetImpliedVolatility(symbol);
            percentageComplete += eachPart;
            if (volatility == null)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Unable to calculate Implied Volatility. Using Historical Volatility...");
                ReportStatus(action, sb.ToString(), StatusType.Warn);
                using (var quantLib = new NativeClassWrapper())
                {
                    var dailyReturns = DataService.GetHistoricalQuote(symbol).Select(p => p.DailyReturn).ToList();
                    volatility = quantLib.GetWeightedStandardDeviation(dailyReturns);
                }
            }
            
            if (volatility == 0)
            {
                ReportStatus(action,string.Format("Unable to estimate volatility for {0}", symbol), StatusType.Fail);
                return null;
            }

            volatility = Math.Round(volatility.Value, 4);
            ReportStatus(action, string.Format("{0}% Finished calculating volatility..", percentageComplete), StatusType.Success);

            int iterations = Convert.ToInt32(DataService.GetAppSettings("iterations", "500000"));
            double price = Math.Round((double)(stock.Ask + stock.Bid)/2, 2);
            double dividendYield = Math.Round(stock.DividendYield.HasValue ? (double) stock.DividendYield.Value : 0, 4);

            using (var quantLib = new NativeClassWrapper())
            {
                ReportStatus(action, "Running simulation...", StatusType.Info);
                double[] result;
                if (dividendYield == 0.0 || rate1Year == 0.0)
                {
                    result =
                    quantLib.SimulateStockPrice(days, iterations, price,
                        volatility.Value).ToArray();
                }
                else
                {
                    result =
                    quantLib.SimulateStockPrice(days, iterations, price, rate1Year, dividendYield,
                        volatility.Value).ToArray();
                    
                }
                if (action == null) return result;
                ReportStatus(action,string.Format("100% Completed"), StatusType.Success);
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("Symbol : {0}", symbol));
                sb.AppendLine(string.Format("Bid price : {0}", stock.Bid));
                sb.AppendLine(string.Format("Ask price : {0}", stock.Ask));
                sb.AppendLine(string.Format("Exchange : {0}", stock.StockExchange));
                sb.AppendLine(string.Format("Volatility %: {0} ", Math.Round(volatility.Value * 100, 2)));
                sb.AppendLine(string.Format("Rsk free Rate %: {0}", Math.Round(rate1Year * 100, 2)));
                sb.AppendLine(string.Format("Dividend %: {0}", dividendYield == 0 ? "Not Available" : Math.Round(dividendYield * 100, 2).ToString()));
                ReportStatus(action,sb.ToString(),StatusType.Info);
                return result;
            }
        }

        private static void ReportStatus(Action<string, StatusType> action, string status, StatusType statusType)
        {
            if (action != null)
            {
                action(status, statusType);
            }
        }
    }
}
