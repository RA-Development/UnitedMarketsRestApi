﻿using System.Collections.Generic;

namespace UnitedMarkets.Core.Entities
{
    public class OriginCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}