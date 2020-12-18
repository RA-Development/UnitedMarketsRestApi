using System;
using System.Collections.Generic;

namespace UnitedMarkets.Core.Entities
{
    public class Order : ISoftDelete
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public double TotalPrice { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        
        public IEnumerable<OrderLine> Products { get; set; }
    }
}