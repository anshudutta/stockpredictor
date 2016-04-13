using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Services
{
    class NasdaqStockEngine
    {
        private const string FtpSite = "ftp://ftp.nasdaqtrader.com/symboldirectory/";
        private const string Nasdaq = "nasdaqlisted.txt";
        private const string Others = "otherlisted.txt";
        private static Dictionary<string, string> _stockTickers;
        private static string _nasdaqListing;
        private static string _otherListing;

        public NasdaqStockEngine()
        {
            _stockTickers = null;
            Init();
        }

        private void Init()
        {
            if (_stockTickers != null) return;

            _stockTickers = new Dictionary<string, string>();
            _nasdaqListing = ProcessFiles(Nasdaq);
            _otherListing = ProcessFiles(Others);

            TrySaveFile(Nasdaq, _nasdaqListing);
            TrySaveFile(Others, _otherListing);

            _stockTickers = _nasdaqListing.ProcessData();
            var otherListed = _otherListing.ProcessData();

            if (otherListed == null) return;

            foreach (var item in otherListed.Where(item => !_stockTickers.ContainsKey(item.Key)))
            {
                _stockTickers.Add(item.Key, item.Value);
            } 

        }

        public Dictionary<string, string> GetStockTickers()
        {
            
            return _stockTickers;
        }

        private static string ProcessFiles(string fileName)
        {
            var fullPath = GetFullFilePath(fileName);
            if (File.Exists(fullPath) && File.GetCreationTime(fullPath) > DateTime.Now.AddDays(-1))
                return File.ReadAllText(GetFullFilePath(fileName));
            File.Delete(fullPath);
            return GetTickers(fileName);
        }

        public List<string> LookUpStock(string search)
        {
            if (_stockTickers == null) return null;
            
            List<string> returnValue = null;
            if (_stockTickers == null) return null;
            var matches = _stockTickers.Where(s => s.Key.ToLower().Contains(search.ToLower()) || s.Value.ToLower().Contains(search.ToLower())).ToArray();

            if (!matches.Any()) return null;

            returnValue = matches.Select(item => new StringBuilder(item.Key).Append("-").Append(item.Value)).Select(sb => sb.ToString()).ToList();
            return returnValue;
        }

        public int IsValidateSymbol(string symbol)
        {
            return _stockTickers == null ? -1 : _stockTickers.ContainsKey(symbol) ? 1 :0;
        }

        private static bool TrySaveFile(string filename, string data)
        {
            try
            {
                var path = GetFullFilePath(filename);
                File.WriteAllText(path, data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private static string GetTickers(string fileName)
        {
            var path = GetFullFilePath(fileName);
            var url = string.Format(FtpSite + fileName);
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            var response = (FtpWebResponse)request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
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
