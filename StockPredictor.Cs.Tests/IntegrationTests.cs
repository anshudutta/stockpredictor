﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using StockPredictor.Services;

namespace StockPredictor.Cs.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [SetUp]
        public void Setup(){}

        [TearDown]
        public void Teardown(){}

        [Test]
        [TestCase("MSFT")]
        [TestCase("GOOG")]
        public void Data_Service_Returns_Volatility(
            string symbol)
        {
            var ds = new DataService();
            var impliedVol = ds.GetImpliedVolatility(symbol);
            Assert.IsTrue(impliedVol > 0);
            Debug.WriteLine(impliedVol);
        }

        [Test]
        [TestCase("CAB.AX")]
        public void Data_Service_Returns_Null_For_NA_Symbol(
            string symbol)
        {
            var ds = new DataService();
            var impliedVol = ds.GetImpliedVolatility(symbol);
            Assert.IsNull(impliedVol);
        }

        [Test]
        [TestCase("MSFT")]
        public void Data_Service_Returns_Valid_Quote(
            string symbol)
        {
            var ds = new DataService();
            var symbols = new List<string>{symbol};

            var quote = ds.GetStockQuote(symbols).First();
            Assert.IsTrue(quote.Bid > 0);
            Assert.IsTrue(quote.Ask > 0);
            Assert.IsTrue(quote.DividendYield > 0);
            
            Debug.WriteLine(quote.Symbol);
            Debug.WriteLine(quote.Bid);
            Debug.WriteLine(quote.Ask);
            Debug.WriteLine(quote.DividendYield);
        }

        [Test]
        [TestCase("MSFT")]
        public void Data_Service_Returns_Valid_Options(
            string symbol)
        {
            var ds = new DataService();

            var options = ds.GetOptionData(symbol);
            foreach (var option in options)
            {
                Assert.IsNotNull(option.ContractName);

                Debug.WriteLine(option.ContractName);
                Debug.WriteLine(option.Bid);
                Debug.WriteLine(option.Ask);
            }
            
        }

        [Test]
        [TestCase("1 MO")]
        [TestCase("3 MO")]
        [TestCase("6 MO")]
        [TestCase("1 YR")]
        [TestCase("2 YR")]
        public void Data_Service_Returns_Yield_Curve(string term)
        {
            var ds = new DataService();
            var yc = ds.GetYieldCurve();
            Assert.IsTrue(yc.Count > 0);
            Assert.IsTrue(yc[term] > 0);
            Debug.WriteLine("{0} {1}", term, yc[term]);
        }

        [Test]
        [TestCase("MSFT",5)]
        public void StockManager_Simulates_Stock_Prices(string symbol, int days)
        {
            var manager = new StockManager {DataService = new DataService()};
            var stockPrices = manager.GetStockProjections(symbol, days, null);
            Assert.IsTrue(stockPrices.Any());
        }

        [Test]
        public void Data_Service_Returns_StockTickers()
        {
            var dataService = new DataService();
            var tickers = dataService.GetStockTickers();
            Assert.IsTrue(tickers.Any());
        }

        [Test]
        [TestCase("goo")]
        [TestCase("msf")]
        public void LookupSymbol(string searchTerm)
        {
            var dataService = new DataService();
            var matches = dataService.LookUpStock(searchTerm);
            Assert.AreEqual(0, dataService.IsValidateSymbol(searchTerm));
            Assert.IsTrue(matches.Any(m => m.ToLower().Contains(searchTerm.ToLower())));
        }

        [Test]
        [TestCase("MSFT")]
        [TestCase("GOOG")]
        public void TestValidSymbol(string symbol)
        {
            var dataService = new DataService();
            Assert.AreEqual(1, dataService.IsValidateSymbol(symbol));
        }
    }
}
