using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using StockPredictor.Model;

namespace StockPredictor.Services
{
    partial class YahooEngine
    {
        public IEnumerable<Option> GetOptionData(string symbol)
        {
            const string url = "https://finance.yahoo.com/";
            var urlParameters = string.Format("q/op?s={0}+Option", symbol);

            var client = new HttpClient { BaseAddress = new Uri(url) };

            var response = client.GetAsync(urlParameters).Result;
            if (!response.IsSuccessStatusCode) return null;

            var dom = response.Content.ReadAsStringAsync().Result;
            return Parse(dom);
        }

        private static IEnumerable<Option> Parse(string dom)
        {
            var optionList = new List<Option>();
            var source = WebUtility.HtmlDecode(dom);
            var resultat = new HtmlDocument();
            resultat.LoadHtml(source);

            var trs = resultat.DocumentNode.Descendants().Where(x => (x.Name == "tr" && x.Attributes["class"] != null &&
                       x.Attributes["class"].Value.Contains("in-the-money"))).ToList();

            foreach (var tr in trs)
            {
                double strike;
                double vol;
                double bid;
                double ask;

                var tds = tr.ChildNodes.Where(d => d.Name == "td");
                var htmlNodes = tds as HtmlNode[] ?? tds.ToArray();
                var strikeHtml = htmlNodes[0].Descendants().First(x => x.Name == "a").InnerHtml;
                var volHtml = htmlNodes[9].Descendants().First(x => x.Name == "div").InnerHtml.Replace("%", string.Empty);
                var contractName = htmlNodes[1].Descendants().First(x => x.Name == "a").InnerHtml;
                var bidHtml = htmlNodes[3].Descendants().First(x => x.Name == "div").InnerHtml;
                var askHtml = htmlNodes[4].Descendants().First(x => x.Name == "div").InnerHtml;

                if (double.TryParse(strikeHtml, out strike) 
                    && double.TryParse(volHtml, out vol) 
                    && double.TryParse(bidHtml, out bid)
                    && double.TryParse(askHtml, out ask)
                    && !string.IsNullOrEmpty(contractName))
                {
                    var option = new Option
                    {
                        Strike = strike,
                        ImpliedVolatility = vol/100,
                        Bid = bid,
                        Ask = ask,
                        ContractName = contractName
                    };
                    optionList.Add(option);
                }

            }

            return optionList;
        }
    }
}
