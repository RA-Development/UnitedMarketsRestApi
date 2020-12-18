using System.Collections.Generic;

namespace UnitedMarkets.Core.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}