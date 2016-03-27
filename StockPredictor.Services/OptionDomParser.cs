using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace StockPredictor.Services
{
    class OptionDomParser
    {
        public static Dictionary<double, double> Parse(string dom)
        {
            var volSmile = new Dictionary<double, double>();
            var source = WebUtility.HtmlDecode(dom);
            var resultat = new HtmlDocument();
            resultat.LoadHtml(source);

            var trs = resultat.DocumentNode.Descendants().Where(x => (x.Name == "tr" && x.Attributes["class"] != null &&
                       x.Attributes["class"].Value.Contains("in-the-money"))).ToList();

            foreach (var tr in trs)
            {
                double strike;
                double vol;
                var tds = tr.ChildNodes.Where(d => d.Name == "td");
                var htmlNodes = tds as HtmlNode[] ?? tds.ToArray();
                var strikeHtml = htmlNodes[0].Descendants().First(x => x.Name == "a").InnerHtml;
                var volHtml = htmlNodes[9].Descendants().First(x => x.Name == "div").InnerHtml.Replace("%", string.Empty);
                if (double.TryParse(strikeHtml, out strike) && double.TryParse(volHtml, out vol) && vol > 0)
                {
                    volSmile[strike] = vol;
                }

            }

            return volSmile;
        }
    }
}
