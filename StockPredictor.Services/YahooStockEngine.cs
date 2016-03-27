﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    partial class YahooEngine
    {
        private const string BaseUrl = "http://query.yahooapis.com/v1/public/yql?q=" +
                                        "select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})" +
                                        "&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

        public IEnumerable<Stock> Fetch(IEnumerable<string> symbols)
        {
            var enumerable = symbols.ToArray();
            var symbolList = String.Join("%2C", enumerable.Select(w => "%22" + w + "%22").ToArray());
            var url = string.Format(BaseUrl,symbolList);
 
            var doc = XDocument.Load(url);
            return Parse(enumerable,doc);
        }
 
        private static IEnumerable<Stock> Parse(IEnumerable<string> symbols, XDocument doc)
        {
            if (doc.Root == null) return null;
            var results = doc.Root.Element("results");
            var quotes = new List<Stock>();
 
            foreach (var symbol in symbols)
            {
                var quote = new Stock {Symbol = symbol};
                var q = results.Elements("quote").First(w => w.Attribute("symbol").Value == quote.Symbol);
 
                quote.Ask = GetDecimal(q.Element("Ask").Value);
                quote.Bid = GetDecimal(q.Element("Bid").Value);
                quote.AverageDailyVolume = GetDecimal(q.Element("AverageDailyVolume").Value);
                quote.BookValue = GetDecimal(q.Element("BookValue").Value);
                quote.Change = GetDecimal(q.Element("Change").Value);
                quote.DividendShare = GetDecimal(q.Element("DividendShare").Value);
                quote.LastTradeDate = GetDateTime(q.Element("LastTradeDate").Value + " " + q.Element("LastTradeTime").Value);
                quote.EarningsShare = GetDecimal(q.Element("EarningsShare").Value);
                quote.EpsEstimateCurrentYear = GetDecimal(q.Element("EPSEstimateCurrentYear").Value);
                quote.EpsEstimateNextYear = GetDecimal(q.Element("EPSEstimateNextYear").Value);
                quote.EpsEstimateNextQuarter = GetDecimal(q.Element("EPSEstimateNextQuarter").Value);
                quote.DailyLow = GetDecimal(q.Element("DaysLow").Value);
                quote.DailyHigh = GetDecimal(q.Element("DaysHigh").Value);
                quote.YearlyLow = GetDecimal(q.Element("YearLow").Value);
                quote.YearlyHigh = GetDecimal(q.Element("YearHigh").Value);
                quote.MarketCapitalization = GetDecimal(q.Element("MarketCapitalization").Value);
                quote.Ebitda = GetDecimal(q.Element("EBITDA").Value);
                quote.ChangeFromYearLow = GetDecimal(q.Element("ChangeFromYearLow").Value);
                quote.PercentChangeFromYearLow = GetDecimal(q.Element("PercentChangeFromYearLow").Value);
                quote.ChangeFromYearHigh = GetDecimal(q.Element("ChangeFromYearHigh").Value);
                quote.LastTradePrice = GetDecimal(q.Element("LastTradePriceOnly").Value);
                quote.PercentChangeFromYearHigh = GetDecimal(q.Element("PercebtChangeFromYearHigh").Value); //missspelling in yahoo for field name
                quote.FiftyDayMovingAverage = GetDecimal(q.Element("FiftydayMovingAverage").Value);
                quote.TwoHunderedDayMovingAverage = GetDecimal(q.Element("TwoHundreddayMovingAverage").Value);
                quote.ChangeFromTwoHundredDayMovingAverage = GetDecimal(q.Element("ChangeFromTwoHundreddayMovingAverage").Value);
                quote.PercentChangeFromTwoHundredDayMovingAverage = GetDecimal(q.Element("PercentChangeFromTwoHundreddayMovingAverage").Value);
                quote.PercentChangeFromFiftyDayMovingAverage = GetDecimal(q.Element("PercentChangeFromFiftydayMovingAverage").Value);
                quote.Name = q.Element("Name").Value;
                quote.Open = GetDecimal(q.Element("Open").Value);
                quote.PreviousClose = GetDecimal(q.Element("PreviousClose").Value);
                quote.ChangeInPercent = GetDecimal(q.Element("ChangeinPercent").Value);
                quote.PriceSales = GetDecimal(q.Element("PriceSales").Value);
                quote.PriceBook = GetDecimal(q.Element("PriceBook").Value);
                quote.ExDividendDate = GetDateTime(q.Element("ExDividendDate").Value);
                quote.PeRatio = GetDecimal(q.Element("PERatio").Value);
                quote.DividendPayDate = GetDateTime(q.Element("DividendPayDate").Value);
                quote.PegRatio = GetDecimal(q.Element("PEGRatio").Value);
                quote.PriceEpsEstimateCurrentYear = GetDecimal(q.Element("PriceEPSEstimateCurrentYear").Value);
                quote.PriceEpsEstimateNextYear = GetDecimal(q.Element("PriceEPSEstimateNextYear").Value);
                quote.ShortRatio = GetDecimal(q.Element("ShortRatio").Value);
                quote.OneYearPriceTarget = GetDecimal(q.Element("OneyrTargetPrice").Value);
                quote.Volume = GetDecimal(q.Element("Volume").Value);
                quote.StockExchange = q.Element("StockExchange").Value;
                quote.LastUpdate = DateTime.Now;
                quote.DividendYield = GetDecimal(q.Element("DividendYield").Value)/100;
                quotes.Add(quote);
            }

            return quotes;
        }
 
        private static decimal? GetDecimal(string input)
        {
            if (input == null) return null;
 
            input = input.Replace("%", "");
 
            decimal value;
 
            if (Decimal.TryParse(input, out value)) return value;
            return null;
        }
 
        private static DateTime? GetDateTime(string input)
        {
            if (input == null) return null;
 
            DateTime value;
 
            if (DateTime.TryParse(input, out value)) return value;
            return null;
        }
    }
}

