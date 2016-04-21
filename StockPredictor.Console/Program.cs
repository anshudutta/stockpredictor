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
        private static readonly StockManager StockManager = new StockManager();
        static void Main(string[] args)
        {
            if (args == null)
            {
                System.Console.WriteLine("Arguments missing - symbol, days");
            }
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Initializing....");
            StockManager.DataService = new DataService();
            var arguments = new Arguments();

            string message;
            if (!IsValidArguments(args, arguments, out message))
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(message);
                return;
            }
            
            System.Console.WriteLine("Processing stock projection for {0} for the next {1} days", arguments.Symbol, arguments.Days);
            System.Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                var points = StockManager.GetStockProjections(arguments.Symbol, arguments.Days, System.Console.WriteLine);
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine("Projected({0}) days = {1}", arguments.Days, Math.Round(points.Last(), 2));

                //Plotter.DoThePlot((x) => x > points.Length - 1 ? 0 : points[(int) x]);
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(string.Format("Something went wrong - {0}", ex.Message));
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static bool IsValidArguments(string[] args, Arguments arguments, out string message)
        {
            bool valid = true;
            var sb = new StringBuilder();

            if (!args.Any())
            {
                message = "You need to provide the following parameters - symbol, days (default 1)";
                return false;
            }

            var symbol = args[0];

            if (args.Count() == 2)
            {
                var strDays = args[1];
                int days = 1;
                if (!int.TryParse(strDays, out days))
                {
                    sb.AppendLine("Invalid argument. No of projection days must be non zero int");
                    valid = false;
                }
                else
                {
                    arguments.Days = days;
                }
            }

            if (StockManager.DataService.IsValidateSymbol(symbol) == 0)
            {
                var lookUp = StockManager.DataService.LookUpStock(symbol);
                if (lookUp == null)
                {
                    sb.AppendLine(string.Format("{0} - Not a valid symbol.", symbol));
                    message = sb.ToString();
                    return false;
                }
                valid = false;
                sb.AppendLine(string.Format("{0} - Not a valid symbol. Did you mean?", symbol));
                foreach (var item in lookUp.Take(5))
                {
                    sb.AppendLine(item);
                }
            }
            else
            {
                arguments.Symbol = symbol;
            }
            message = sb.ToString();
            return valid;
        }
    }
}
