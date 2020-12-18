using System.Collections.Generic;

namespace UnitedMarkets.Core.Entities
{
    public class Origin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}