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

        public double[] GetStockProjections(string symbol, int days, Action<string> action)
        {
            var percentageComplete = 0;
            const int eachPart = 20;
            if (action != null)
            {
                action(string.Format("{0}% Downloading yield curve..", percentageComplete));
            }
            double rate1Year = DataService.GetYieldCurve()["1 YR"];
            percentageComplete += eachPart;

            if (action != null)
            {
                action(string.Format("{0}% Downloading stock quote..", percentageComplete)); 
            }
            var stock = DataService.GetStockQuote(new List<string> {symbol}).First();
            percentageComplete += eachPart;

            if (action != null)
            {
                action(string.Format("{0}% Calculating implied volatility..", percentageComplete)); 
            }
            double? volatility = DataService.GetImpliedVolatility(symbol);
            percentageComplete += eachPart;

            int iterations = Convert.ToInt32(DataService.GetAppSettings("iterations", "1000000"));
            double price = (double)(stock.Ask + stock.Bid)/2;
            double dividendYield = stock.DividendYield.HasValue ? (double) stock.DividendYield.Value : 0;

            if (volatility == null)
            {
                if (action != null)
                {
                    action("Unable to calculate Implied Volatility. Using Historical Volatility...");
                }
                using (var quantLib = new NativeClassWrapper())
                {
                    if (action != null)
                    {
                        action("Calculating one year historical return..");
                    }
                    var dailyReturns = DataService.GetHistoricalQuote(symbol).Select(p => p.DailyReturn).ToList();
                    volatility = quantLib.GetWeightedStandardDeviation(dailyReturns);
                }
            }

            if (volatility == null)
            {
                throw new Exception(string.Format("Unable to gather volatility for {0}", symbol));
            }
            
            using (var quantLib = new NativeClassWrapper())
            {
                if (action != null)
                {
                    action(string.Format("{0}% Running Simulation..", percentageComplete));
                }
                double[] result;
                if ((int)dividendYield != 0 || (int)rate1Year != 0)
                {
                    result =
                    quantLib.SimulateStockPrice(days, iterations, price, rate1Year, dividendYield,
                        volatility.Value).ToArray();
                }
                else
                {
                    result =
                    quantLib.SimulateStockPrice(days, iterations, price,
                        volatility.Value).ToArray();
                }
                if (action != null)
                {
                    action(string.Format("{0}% Completed", 100));
                    action("\n");
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format("Symbol : {0}", symbol));
                    sb.AppendLine(string.Format("Bid price : {0}", stock.Bid));
                    sb.AppendLine(string.Format("Ask price : {0}", stock.Ask));
                    sb.AppendLine(string.Format("Parameters : Volatility : {0}, Rsk free Rate : {1}, Dividend : {2} ", volatility, rate1Year, (int)dividendYield == 0? "Not Available": dividendYield.ToString()));
                    action(sb.ToString());
                }
                return result;
            }
        }

        //private static double[] GetSimulationResult(int days, long iterations, double currentPrice, double rate, double dividendYield, double volatility)
        //{
        //    return QuantLibraryWrapper.SimulateStockPrice(days, iterations, currentPrice, rate, dividendYield, volatility);
        //}

        //public void Dispose()
        //{
        //    Marshal.ReleaseComObject(QuantLib);
        //}
    
    }
}
