using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgTrade.Models.Info
{
    public class StockChart
    {
        //DateTimeUtc
        public long A { get; set; }
        //Open
        public decimal B { get; set; }
        //High
        public decimal C { get; set; }
        //Low
        public decimal D { get; set; }
        //Close
        public decimal E { get; set; }
        public string Symbol { get; set; }
    }
}