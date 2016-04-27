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
            
            //System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Warming up....");
            StockManager.DataService = new DataService();
            var arguments = new Arguments();

            if (args == null || !args.Any())
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Arguments missing - symbol, days");
                System.Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            string message;
            if (!IsValidArguments(args, arguments, out message))
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(message);
                return;
            }
            
            System.Console.WriteLine("Projecting {0} for the next {1} days", arguments.Symbol, arguments.Days);
            System.Console.ForegroundColor = ConsoleColor.Green;
            try
            {
                var points = StockManager.GetStockProjections(arguments.Symbol, arguments.Days, (s, st) =>
                {
                    switch (st)
                    {
                        case StatusType.Info:
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case StatusType.Success:
                            System.Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case StatusType.Warn:
                            System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                            break;
                        case StatusType.Fail:
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }
                    System.Console.WriteLine(s);
                });
                if (points != null && points.Any())
                {
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.WriteLine("Projected({0}) days = {1}", arguments.Days, Math.Round(points.Last(), 2));
                }
                //Plotter.DoThePlot((x) => x > points.Length - 1 ? 0 : points[(int) x]);
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Something went wrong - {0}", ex.Message);
            }
            finally
            {
                System.Console.ForegroundColor = ConsoleColor.Gray;
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

            arguments.Symbol = args[0];

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
            message = sb.ToString();
            return valid;
        }
    }
}
