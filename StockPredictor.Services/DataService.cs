using System.Collections.Generic;
using System.Linq;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Quote> GetStockQuote(List<string> quotes)
        {
            var engine = new YahooEngine();
            return engine.Fetch(quotes);
        }

        public Dictionary<double, double> GetVolatilitySmile(string symbol)
        {
            var engine = new YahooEngine();
            return engine.GetOptionData(symbol);
        }

        public double GetImpliedVolatility(string symbol)
        {
            var volSmile = GetVolatilitySmile(symbol);
            return volSmile == null || volSmile.Count == 0 ? 0 : volSmile.Average(kv => kv.Value)/100;
        }

        public Dictionary<string, double> GetYieldCurve()
        {
            var engine = new QuandlEngine();
            return engine.GetYieldCurve();
        }
    }
}
