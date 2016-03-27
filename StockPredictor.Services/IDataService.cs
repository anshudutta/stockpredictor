using System.Collections.Generic;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    public interface IDataService
    {
        IEnumerable<Stock> GetStockQuote(List<string> quotes);
        IEnumerable<Option> GetOptionData(string symbol);
        double? GetImpliedVolatility(string symbol);
        Dictionary<string, double> GetYieldCurve();
    }
}
