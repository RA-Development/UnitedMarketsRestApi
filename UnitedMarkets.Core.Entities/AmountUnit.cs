﻿using System.Collections.Generic;

namespace UnitedMarkets.Core.Entities
{
    public class AmountUnit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}