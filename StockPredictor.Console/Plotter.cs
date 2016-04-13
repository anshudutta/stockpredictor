using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Console
{
    class Plotter
    {
        const char Blank = ' ';
        const char Dot = '.';
        const char X = 'x';
        const int CMaxLineChars = 79;
        const int CHalf = CMaxLineChars / 2;
        static readonly char[] Line = new char[CMaxLineChars];

        internal delegate double Func(double x);

        private static void PlotX(int[] xPoints)
        {
            const int margin = 5;
            int x0 = System.Console.WindowLeft + margin;
            int y0 = System.Console.WindowHeight + margin;
            System.Console.SetCursorPosition(x0, y0);
            var windowWidth = System.Console.WindowWidth;
            int marker = (windowWidth - 2*margin)/xPoints.Length < 1
                ? xPoints.Length/(windowWidth - 2*margin)
                : (windowWidth - 2*margin)/xPoints.Length;

            for (int x = x0; x < xPoints.Length * 5 + margin; x++)
            {
                System.Console.SetCursorPosition(x, y0);
                System.Console.WriteLine('_');
                if (x>x0 && (x - x0) % marker == 0)
                {
                    System.Console.SetCursorPosition(x, y0);
                    System.Console.WriteLine("|");
                }
            };
        }

        public static void DoThePlot(Func del)
        {
            PlotX(new []{0,1,2,3,4,5});
            
            FillUp(Line, withChar: Dot); // line of dots for "vertical" axis
            System.Console.WriteLine(Line);
            FillUp(Line, withChar: Blank); // clear the line
            PlotFunc(del);
        }

        private static void FillUp(char[] line, char withChar = '\0')
        {
            for (int i = 0; i < line.Length; i++)
            {
                line[i] = withChar;
            }
        }

        private static void PlotFunc(Func f)
        {
            double maxval = 9.0; //arbitrary values
            double delta = 0.2; //size of iteration steps
            int loc;
            Line[CHalf] = Dot; // for "horizontal" axis
            for (double x = 0.0001; x < maxval; x += delta) //0.0001 to avoid DIV/0 error
            {
                loc = (int)Math.Round(f(x) * CHalf) + CHalf;
                Line[loc] = X;
                System.Console.WriteLine(Line);
                FillUp(Line, withChar: Blank); // blank the line, remove X point
                Line[CHalf] = Dot; // for horizontal axis
            }
        }
    }
}
