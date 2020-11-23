using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Filtering
{
    public class FilteredList<T>
    {

        public Filter FilterUsed { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> List { get; set; }

    }
}
