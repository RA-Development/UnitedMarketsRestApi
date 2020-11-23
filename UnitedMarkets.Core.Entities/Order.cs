using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Entities
{
    public class Order
    {

        public int Id { get; set; }
        public List<OrderLine> Products { get; set; }


    }
}
