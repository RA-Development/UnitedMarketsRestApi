using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private UnitedMarketsDbContext _ctx;

        public DbInitializer(UnitedMarketsDbContext dbContext)
        {
            _ctx = dbContext;
        }

        public void InitData()
        {
            //    init amountUnits
            _ctx.AmountUnits.Add(new AmountUnit() {Name = "count"});
            _ctx.AmountUnits.Add(new AmountUnit() {Name = "kg"});
            _ctx.SaveChanges();

            //    init markets
            _ctx.Markets.Add(new Market() {Name = "Netto"});
            _ctx.Markets.Add(new Market() {Name = "Fakta"});
            _ctx.Markets.Add(new Market() {Name = "føtex"});
            _ctx.SaveChanges();
            //    init origins
            _ctx.OriginCountries.Add(new OriginCountry() {Name = "Spain"});
            _ctx.OriginCountries.Add(new OriginCountry() {Name = "Italy"});
            _ctx.SaveChanges();
            //    init categories
            _ctx.Categories.Add(new Category() {Name = "Fruit & Vegetable"});
            _ctx.Categories.Add(new Category() {Name = "Eggs & Dairy"});
            _ctx.Categories.Add(new Category() {Name = "Baverages"});
            _ctx.SaveChanges();
            var product1 = new Product()
            {
                Name = "Apples 6 pack",
                CategoryId = 1,
                MarketId = 1,
                OriginCountryId = 1,
                Price = 4.95,
                Amount = 3,
                AmountUnitId = 1
            };

            var product2 = new Product()
            {
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 1,
                OriginCountryId = 2,
                Price = 3.00,
                Amount = 2,
                AmountUnitId = 2
            };

            var product3 = new Product()
            {
                Name = "Grape",
                CategoryId = 1,
                MarketId = 2,
                OriginCountryId = 1,
                Price = 6.00,
                Amount = 3,
                AmountUnitId = 1
            };

            var product4 = new Product()
            {
                Name = "Banana",
                CategoryId = 1,
                MarketId = 1,
                OriginCountryId = 1,
                Price = 6.00,
                Amount = 3,
                AmountUnitId = 1
            };
            var product5 = new Product()
            {
                Name = "Kiwi",
                CategoryId = 1,
                MarketId = 1,
                OriginCountryId = 1,
                Price = 6.00,
                Amount = 3,
                AmountUnitId = 1
            };
            var product6 = new Product()
            {
                Name = "Coconut",
                CategoryId = 1,
                MarketId = 1,
                OriginCountryId = 1,
                Price = 6.00,
                Amount = 3,
                AmountUnitId = 1
            };

            _ctx.Products.Add(product1);
            _ctx.Products.Add(product2);
            _ctx.Products.Add(product3);
            _ctx.Products.Add(product4);
            _ctx.Products.Add(product5);
            _ctx.Products.Add(product6);


            _ctx.SaveChanges();
        }
    }
}