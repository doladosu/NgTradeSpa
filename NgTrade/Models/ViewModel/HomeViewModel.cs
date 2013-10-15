﻿using System.Collections.Generic;
using NgTrade.Models.Data;

namespace NgTrade.Models.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Quote> DayGainers { get; set; }
        public IEnumerable<Quote> DayLosers { get; set; }
        public string SQuoteDay { get; set; }
    }
}