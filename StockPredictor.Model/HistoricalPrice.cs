using System;

namespace StockPredictor.Model
{
    public class HistoricalPrice
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        public double AdjClose { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Symbol : {0}, Date : {1}, OPen : {2}, High :{3}, Low : {4}, CLose : {5}, Volume : {6}, AdjClose : {7}", Symbol,
                    Date.ToString("yyyy-MM-dd"), Open, High, Low, Close, Volume, AdjClose);
        }
    }
}
