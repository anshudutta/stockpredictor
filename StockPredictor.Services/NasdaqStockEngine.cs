using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Services
{
    public class NasdaqStockEngine
    {
        private const string FtpSite = "ftp://ftp.nasdaqtrader.com/symboldirectory/";
        private const string Nasdaq = "nasdaqlisted.txt";
        private const string Others = "otherlisted.txt";
        private static Dictionary<string, string> _stockTickers;

        public NasdaqStockEngine()
        {
            _stockTickers = null;
        }

        public Dictionary<string, string> GetStockTickers()
        {
            if (_stockTickers != null) return _stockTickers;

            _stockTickers = new Dictionary<string, string>();
            ProcessFiles(Nasdaq);
            ProcessFiles(Others);

            if (File.Exists(GetFullFilePath(Nasdaq)))
            {
                _stockTickers = File.ReadAllText(GetFullFilePath(Nasdaq)).ProcessData();
            }

            if (!File.Exists(GetFullFilePath(Others))) return _stockTickers;
            var otherListed = File.ReadAllText(GetFullFilePath(Nasdaq)).ProcessData();
            foreach (var item in otherListed.Where(item => !_stockTickers.ContainsKey(item.Key)))
            {
                _stockTickers.Add(item.Key, item.Value);
            }

            return _stockTickers;
        }

        private static void ProcessFiles(string fileName)
        {
            var fullPath = GetFullFilePath(fileName);
            if (File.Exists(fullPath) && File.GetCreationTime(fullPath) > DateTime.Now.AddDays(-1)) return;
            File.Delete(fullPath);
            DownloadTickers(fileName);
        }

        public List<string> LookUpStock(string search)
        {
            if (_stockTickers == null)
            {
                GetStockTickers();
            }

            List<string> returnValue = null;
            if (_stockTickers == null) return null;
            var matches = _stockTickers.Where(s => s.Key.ToLower().Contains(search.ToLower()) || s.Value.ToLower().Contains(search.ToLower())).ToArray();

            if (!matches.Any()) return null;

            returnValue = matches.Select(item => new StringBuilder(item.Key).Append("-").Append(item.Value)).Select(sb => sb.ToString()).ToList();
            return returnValue;
        }

        private static void DownloadTickers(string fileName)
        {
            var path = GetFullFilePath(fileName);
            var url = string.Format(FtpSite + fileName);
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            var response = (FtpWebResponse)request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return;
                using (var reader = new StreamReader(responseStream))
                {
                    string data = reader.ReadToEnd();
                    File.WriteAllText(path, data);
                }
            }  
        }

        public static string GetFullFilePath(string fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = string.Format(baseDir + @"\" + fileName);
            return path;
        }
    }
}
