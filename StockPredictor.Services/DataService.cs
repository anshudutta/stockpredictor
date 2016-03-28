using System.Collections.Generic;
using System.Linq;
using StockPredictor.Model;
using System.Configuration;

namespace StockPredictor.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Stock> GetStockQuote(List<string> quotes)
        {
            var engine = new YahooEngine();
            return engine.Fetch(quotes);
        }

        public IEnumerable<Option> GetOptionData(string symbol)
        {
            var engine = new YahooEngine();
            return engine.GetOptionData(symbol);
        }

        public double? GetImpliedVolatility(string symbol)
        {
            var options = GetOptionData(symbol) ;
            var enumerable = options as Option[] ?? options.ToArray();
            return options == null || !enumerable.Any() ? (double?) null : enumerable.Average(o => o.ImpliedVolatility) / 100;
        }

        public Dictionary<string, double> GetYieldCurve()
        {
            var engine = new QuandlEngine();
            return engine.GetYieldCurve();
        }

        public string GetAppSettings(string key, string defaultValue)
        {
            var returnValue = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(returnValue) ? defaultValue : returnValue;
        }
    }
}
