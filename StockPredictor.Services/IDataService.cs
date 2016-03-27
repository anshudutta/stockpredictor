using System.Collections.Generic;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    public interface IDataService
    {
        IEnumerable<Quote> GetStockQuote(List<string> quotes);
        Dictionary<double, double> GetVolatilitySmile(string symbol);
        double GetImpliedVolatility(string symbol);
        Dictionary<string, double> GetYieldCurve();
    }
}
