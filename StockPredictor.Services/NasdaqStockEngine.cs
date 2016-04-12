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

        public Dictionary<string, string> GetStockTickers()
        {
            var list = DownloadTickers("nasdaqlisted.txt").ProcessData();
            var otherListed = DownloadTickers("otherlisted.txt").ProcessData();

            foreach (var item in otherListed.Where(item => !list.ContainsKey(item.Key)))
            {
                list.Add(item.Key, item.Value);
            }
            return list;
        }

        private static string DownloadTickers(string fileName)
        {
            string data = null;
            var url = string.Format(FtpSite + fileName);
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            //request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            var response = (FtpWebResponse)request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return null;
                using (var reader = new StreamReader(responseStream))
                {
                    data = reader.ReadToEnd();
                }
            }
            //var responseStream = response.GetResponseStream();
            //if (responseStream != null)
            //{
            //    var reader = new StreamReader(responseStream);
            //    data = reader.ReadToEnd();
            //    reader.Close();
            //}
            //response.Close();  
            return data;
        }

        //private static Dictionary<string, string> ProcessData(string data)
        //{
        //    var lines = data.Split('\n');
        //    lines = lines.Skip(1).Take(lines.Length - 1).ToArray();
        //    var symbolList = new Dictionary<string, string>();
        //    foreach (var line in lines)
        //    {
        //        var segment = line.Split('|');
        //        symbolList[segment[0]] = segment[1];
        //    }

        //    return symbolList;
        //}
    }
}
