using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Filtering
{
    public class Filter
    {
        public int CurrentPage { get; set; }
        public int ItemsPrPage { get; set; }
        public string SearchField { get; set; }
        public string SearchValue { get; set; }
        public int MarketId { get; set; }
    }
}