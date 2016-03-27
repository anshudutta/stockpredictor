﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Teardoen(){}

        [Test]
        public void Data_Service_Returns_Volatility()
        {
            var ds = new DataService();
            double impliedVol = ds.GetImpliedVolatility("MSFT");
            Assert.IsTrue(impliedVol > 0);
            Debug.WriteLine(impliedVol);
            impliedVol = ds.GetImpliedVolatility("GOOG");
            Assert.IsTrue(impliedVol > 0);
            Debug.WriteLine(impliedVol);
            Assert.IsTrue(ds.GetImpliedVolatility("CAB.AX") == 0.0);
        }

        [Test]
        public void Data_Service_Returns_Valid_Quote()
        {
            var ds = new DataService();
            var symbols = new List<string>()
            {
                "MSFT"
            };

            var quote = ds.GetStockQuote(symbols).First();
            Assert.IsTrue(quote.Bid > 0);
            Assert.IsTrue(quote.Ask > 0);
            Assert.IsTrue(quote.DividendYield > 0);
            
            Debug.WriteLine(quote.Symbol);
            Debug.WriteLine(quote.Bid);
            Debug.WriteLine(quote.Ask);
            Debug.WriteLine(quote.DividendYield);
        }
    }
}