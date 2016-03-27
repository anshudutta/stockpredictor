using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Model
{
    public class Option
    {
        public string ContractName { get; set; }
        public double Strike { get; set; }
        public double ImpliedVolatility { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
}
