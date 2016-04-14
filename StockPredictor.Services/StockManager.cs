using System;
using System.Collections.Generic;
using System.Linq;
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
            double? impliedVolatility = DataService.GetImpliedVolatility(symbol);
            percentageComplete += eachPart;

            int iterations = Convert.ToInt32(DataService.GetAppSettings("iterations", "10000000"));
            double price = (double)(stock.Ask + stock.Bid)/2;
            double dividendYield = stock.DividendYield.HasValue ? (double) stock.DividendYield.Value : 0;

            if (impliedVolatility.HasValue)
            {
                if (action != null)
                {
                    action(string.Format("{0}% Running Simulation..", percentageComplete));
                }
                
                using (var quantLib = new NativeClassWrapper())
                {
                    var result = quantLib.SimulateStockPrice(days, iterations, price, rate1Year, dividendYield, impliedVolatility.Value).ToArray();
                    percentageComplete += eachPart;
                    if (action != null)
                    {
                        action(string.Format("{0}% Completed", 100));
                        action("\n");
                        var sb = new StringBuilder();
                        sb.AppendLine(string.Format("Symbol : {0}", symbol));
                        sb.AppendLine(string.Format("Bid price : {0}", stock.Bid));
                        sb.AppendLine(string.Format("Ask price : {0}", stock.Ask));
                        action(sb.ToString());
                    }
                    return result;
                }
            }
            //else
            //{
            //    // historical Volatility
            //}
            return null;
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
