using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace StockProjector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var test1 = new Test() {Name = "abc", Number = 1};

            var test2 = test1;

            //test2.Name = "def";
            test2.Number = 2;

        }
    }

    class Test
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
