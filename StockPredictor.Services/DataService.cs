using System;
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

        //public double? GetHistoricalVolatility(string symbol)
        //{
        //    var historicalPrices = GetHistoricalQuote(symbol).OrderBy(h=> h.Date).ToList();
        //    var dailyReturns = new List<double> {0};
        //    for (int i = 1; i < dailyReturns.Count; i++)
        //    {
        //        var dailyreturn = Math.Log(historicalPrices[i].Close/historicalPrices[i - 1].Close);
        //    }
        //    return options == null || !enumerable.Any() ? (double?)null : enumerable.Average(o => o.ImpliedVolatility) / 100;
        //}

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

        public Dictionary<string, string> GetStockTickers()
        {
            var nasdaqStock = new NasdaqStockEngine();
            return nasdaqStock.GetStockTickers();
        }

        public List<string> LookUpStock(string search)
        {
            var nasdaqStock = new NasdaqStockEngine();
            return nasdaqStock.LookUpStock(search);
        }

        public int IsValidateSymbol(string symbol)
        {
            var nasdaqStock = new NasdaqStockEngine();
            return nasdaqStock.IsValidateSymbol(symbol);
        }

        public IEnumerable<HistoricalPrice> GetHistoricalQuote(string quote)
        {
            var yahooEngine = new YahooEngine();
            var prices = yahooEngine.FetchHistorical(quote).OrderBy(h => h.Date).ToList();
            prices[0].DailyReturn = 0;
            for (int i = 1; i < prices.Count(); i++)
            {
                prices[i].DailyReturn = Math.Log(prices[i].Close/prices[i - 1].Close);
            }

            return prices;
        }
    }
}
