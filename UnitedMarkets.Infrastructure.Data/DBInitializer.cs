using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data
{
    public class DBInitializer : IDBInitializer
    {
        private UnitedMarketsDBContext _ctx;

        public DBInitializer(UnitedMarketsDBContext dbContext)
        {
            _ctx = dbContext;
        }

        public void InitData()
        {
            //    init markets
            var marketNetto = _ctx.Markets.Add(new Market() {Name = "Netto"}).Entity;
            var marketFakta = _ctx.Markets.Add(new Market() {Name = "Fakta"}).Entity;
            var marketFøtex = _ctx.Markets.Add(new Market() {Name = "føtex"}).Entity;
            _ctx.SaveChanges();

            //    init origins
            var originSpain = _ctx.Origins.Add(new Origin() {Name = "Spain"}).Entity;
            var originItaly = _ctx.Origins.Add(new Origin() {Name = "Italy"}).Entity;
            _ctx.SaveChanges();

            //    init categories
            var catFruitandVeggie = _ctx.Categories.Add(
                new Category() {Name = "Fruit & Vegetable"}).Entity;
            var catEggsAndDairy = _ctx.Categories.Add(
                new Category() {Name = "Eggs & Dairy"}).Entity;
            var catBeverages = _ctx.Categories.Add(
                new Category() {Name = "Baverages"}).Entity;
            _ctx.SaveChanges();

            var product1 = new Product()
            {
                Name = "Apple",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                Price = 4.95,
            };
            _ctx.Products.Add(product1);
            _ctx.SaveChanges();
        }
    }
}