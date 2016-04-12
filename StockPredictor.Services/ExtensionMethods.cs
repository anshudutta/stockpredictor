using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Services
{
    public static class ExtensionMethods
    {
        public static Dictionary<string, string> ProcessData(this string data)
        {
            var lines = data.Split('\n');
            lines = lines.Skip(1).Take(lines.Length - 1).ToArray();
            var symbolList = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var segment = line.Split('|');
                if (segment.Any())
                {
                    symbolList[segment[0]] = segment[1];
                }
            }

            return symbolList;
        }
    }
}
