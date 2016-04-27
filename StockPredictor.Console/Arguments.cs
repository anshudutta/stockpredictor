using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Console
{
    class Arguments
    {
        public Arguments()
        {
            Days = 1;
        }
        public string Symbol { get; set; }
        public int Days { get; set; }
    }
}
