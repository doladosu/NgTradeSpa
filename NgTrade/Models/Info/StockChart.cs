namespace NgTrade.Models.Info
{
    public class StockChart
    {
        public long DateTimeUtc { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public string Symbol { get; set; }
    }
}