using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<Quote> GetStockQuote(List<string> quotes)
        {
            var engine = new YahooStockEngine();
            return engine.Fetch(quotes);
        }

        public Dictionary<double, double> GetVolatilitySmile(string symbol)
        {
            const string url = "https://finance.yahoo.com/";
            var urlParameters = string.Format("q/op?s={0}+Option", symbol);

            var client = new HttpClient { BaseAddress = new Uri(url) };

            var response = client.GetAsync(urlParameters).Result;
            if (!response.IsSuccessStatusCode) return null;
            // Parse the response body. Blocking!
            var dom = response.Content.ReadAsStringAsync().Result;
            return OptionDomParser.Parse(dom);
        }

        public double GetImpliedVolatility(string symbol)
        {
            var volSmile = GetVolatilitySmile(symbol);
            return volSmile == null || volSmile.Count == 0 ? 0 : volSmile.Average(kv => kv.Value)/100;
        }

        public double GetRiskFreeRate()
        {
            throw new NotImplementedException();
        }
    }
}
