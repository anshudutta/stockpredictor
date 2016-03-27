using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace StockPredictor.Services
{
    class QuandlEngine
    {
        public Dictionary<string, double> GetYieldCurve()
        {
            const string url = "https://www.quandl.com/";
            const string urlParams = "api/v3/datasets/USTREASURY/YIELD.json?api_key=xJ5VQHGTUXUzfsnecaSu";

            var client = new HttpClient { BaseAddress = new Uri(url) };

            var response = client.GetAsync(urlParams).Result;
            if (!response.IsSuccessStatusCode) return null;

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var jobject = JObject.Parse(jsonString);
            var yieldCurve = new Dictionary<string, double>();

            var columns = jobject["dataset"]["column_names"];
            var dataSet = jobject["dataset"]["data"][0];

            for (int i = 1; i < columns.Count(); i++)
            {
                yieldCurve[columns[i].Value<string>()] = dataSet[i].Value<double>();
            }
            return yieldCurve;
        }
    }
}
