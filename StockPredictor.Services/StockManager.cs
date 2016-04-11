using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockPredictorManagedWrapper;

namespace StockPredictor.Services
{
    public class StockManager
    {
        //private static readonly NativeClassWrapper QuantLib = new NativeClassWrapper();
        public IDataService DataService { get; set; }

        public StockManager()
        {
            
        }

        public double[] GetStockProjections(string symbol, int days)
        {
            double rate1Year = DataService.GetYieldCurve()["1 YR"];
            var stock = DataService.GetStockQuote(new List<string> {symbol}).First();
            double? impliedVolatility = DataService.GetImpliedVolatility(symbol);
            int iterations = Convert.ToInt32(DataService.GetAppSettings("iterations", "10000000"));
            double price = (double)(stock.Ask + stock.Bid)/2;
            double dividendYield = stock.DividendYield.HasValue ? (double) stock.DividendYield.Value : 0;

            if (impliedVolatility.HasValue)
            {
                using (var quantLib = new NativeClassWrapper())
                {
                    return quantLib.SimulateStockPrice(days, iterations, price, rate1Year, dividendYield, impliedVolatility.Value).ToArray();
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
