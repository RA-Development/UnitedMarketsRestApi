using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Entities
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public List<OrderLine> Orders { get; set; }



    }
}
