using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPredictor.Services;

namespace StockPredictor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var symbol = args[0];
            int days = 1;
            if (args.Count() == 2)
            {
                var strDays = args[1];
                if (!int.TryParse(strDays, out days))
                {
                    System.Console.WriteLine("Invalid argument. No of projection days must be non zero int");
                }
            }
            
            var stockManager = new StockManager();
            var points = stockManager.GetStockProjections(symbol, days);
            
        }
    }
}
