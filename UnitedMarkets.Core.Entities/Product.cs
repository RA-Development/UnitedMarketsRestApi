using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedMarkets.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PricePerUnit { get; set; }
        //    TODO: Remove from DB but has to stay here
        public double Price { get; set; }
        public int Amount { get; set; }
        public IEnumerable<OrderLine> Orders { get; set; }
        public int MarketId { get; set; }
        public Market Market { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int OriginId { get; set; }
        public Origin Origin { get; set; }
        public int AmountUnitId { get; set; }
        public AmountUnit AmountUnit { get; set; }
    }
}