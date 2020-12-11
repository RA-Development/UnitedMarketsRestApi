using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Entities
{
    public class OrderLine
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double SubTotalPrice { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}