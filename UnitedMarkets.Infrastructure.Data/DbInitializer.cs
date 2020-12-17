using System;
using System.Collections.Generic;
using UnitedMarkets.Core.ApplicationServices.HelperServices;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly UnitedMarketsDbContext _ctx;
        private readonly IRepository<Market> _marketRepository;

        public DbInitializer(
            UnitedMarketsDbContext ctx,
            IRepository<Market> marketRepository,
            IAuthenticationHelper authenticationHelper)
        {
            _ctx = ctx;
            _marketRepository = marketRepository;
            _authenticationHelper = authenticationHelper;
        }

        public void InitData()
        {
            //    init amountUnits
            InitAmountUnits();

            //    init markets
            InitMarkets();

            //    init origins
            InitOrigins();

            //    init categories
            InitCategories();

            //    init products
            InitProducts();

            //    init users
            InitUsers();

            //    init status
            InitStatus();

            //    init orders
            InitOrders();

            //    init order lines
            InitOrderLines();
        }

        private void InitAmountUnits()
        {
            _ctx.AmountUnits.Add(new AmountUnit {Name = "piece"});
            _ctx.AmountUnits.Add(new AmountUnit {Name = "kg"});
        }

        private void InitMarkets()
        {
            _marketRepository.Create(new Market {Name = "Fakta"});
            _marketRepository.Create(new Market {Name = "Netto"});
            _marketRepository.Create(new Market {Name = "Irma"});
            _marketRepository.Create(new Market {Name = "Føtex"});
            _marketRepository.Create(new Market {Name = "Kvickly"});
            _marketRepository.Create(new Market {Name = "Bilka"});
        }

        private void InitOrigins()
        {
            _ctx.Origins.Add(new Origin {Name = "Spain"});
            _ctx.Origins.Add(new Origin {Name = "Italy"});
        }

        private void InitCategories()
        {
            _ctx.Categories.Add(new Category {Name = "Fruit & Vegetable"});
            _ctx.Categories.Add(new Category {Name = "Eggs & Dairy"});
            _ctx.Categories.Add(new Category {Name = "Beverages"});
        }

        private void InitProducts()
        {
            var product1 = new Product
            {
                Name = "Apples",
                CategoryId = 1,
                MarketId = 2,
                OriginId = 1,
                PricePerUnit = 5,
                Amount = 6,
                AmountUnitId = 1
            };

            var product2 = new Product
            {
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 3,
                OriginId = 2,
                PricePerUnit = 3.10,
                Amount = 2,
                AmountUnitId = 2
            };

            var product3 = new Product
            {
                Name = "Grape",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 6.00,
                Amount = 1,
                AmountUnitId = 2
            };

            var product4 = new Product
            {
                Name = "Banana",
                CategoryId = 1,
                MarketId = 3,
                OriginId = 1,
                PricePerUnit = 9.50,
                Amount = 4,
                AmountUnitId = 1
            };
            var product5 = new Product
            {
                Name = "Kiwi",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 6.00,
                Amount = 1,
                AmountUnitId = 1
            };
            var product6 = new Product
            {
                Name = "Coconut",
                CategoryId = 1,
                MarketId = 2,
                OriginId = 1,
                PricePerUnit = 6.00,
                Amount = 1,
                AmountUnitId = 1
            };
            var product7 = new Product
            {
                Name = "Melon",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 19.00,
                Amount = 1,
                AmountUnitId = 1
            };
            var product8 = new Product
            {
                Name = "Red bell pepper",
                CategoryId = 1,
                MarketId = 3,
                OriginId = 2,
                PricePerUnit = 7.00,
                Amount = 1,
                AmountUnitId = 1
            };
            var product9 = new Product
            {
                Name = "Broccoli",
                CategoryId = 1,
                MarketId = 2,
                OriginId = 1,
                PricePerUnit = 14.00,
                Amount = 1,
                AmountUnitId = 1
            };


            _ctx.Products.Add(product1);
            _ctx.Products.Add(product2);
            _ctx.Products.Add(product3);
            _ctx.Products.Add(product4);
            _ctx.Products.Add(product5);
            _ctx.Products.Add(product6);
            _ctx.Products.Add(product7);
            _ctx.Products.Add(product8);
            _ctx.Products.Add(product9);
        }

        private void InitUsers()
        {
            const string password = "abcd";
            _authenticationHelper.CreatePasswordHash(password, out var passwordHashJohn, out var passwordSaltJohn);
            _authenticationHelper.CreatePasswordHash(password, out var passwordHashJane, out var passwordSaltJane);

            var users = new List<User>
            {
                new User
                {
                    Username = "UserJohnDoe",
                    PasswordHash = passwordHashJohn,
                    PasswordSalt = passwordSaltJohn,
                    IsAdmin = false
                },
                new User
                {
                    Username = "AdminJaneDoe",
                    PasswordHash = passwordHashJane,
                    PasswordSalt = passwordSaltJane,
                    IsAdmin = true
                }
            };

            _ctx.Users.AddRange(users);
            _ctx.SaveChanges();
        }

        private void InitStatus()
        {
            _ctx.Status.Add(new Status {Name = "Cancelled"});
            _ctx.Status.Add(new Status {Name = "Shipped"});
            _ctx.Status.Add(new Status {Name = "Confirmed"});
            _ctx.Status.Add(new Status {Name = "Pending"});

            _ctx.SaveChanges();
        }

        private void InitOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, Products = new List<OrderLine>(), DateCreated = DateTime.Now.AddDays(-12),
                    TotalPrice = 42,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", StatusId = 2
                },
                new Order
                {
                    Id = 2, Products = new List<OrderLine>(), DateCreated = DateTime.Now.AddDays(-2),
                    TotalPrice = 222.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", StatusId = 3
                },
                new Order
                {
                    Id = 3, Products = new List<OrderLine>(), DateCreated = DateTime.Now, TotalPrice = 1255.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", StatusId = 4
                }
            };

            _ctx.Orders.AddRange(orders);
            _ctx.SaveChanges();
        }

        private void InitOrderLines()
        {
            _ctx.OrderLines.Add(new OrderLine {ProductId = 1, OrderId = 1, Quantity = 2, SubTotalPrice = 36});
            _ctx.OrderLines.Add(new OrderLine {ProductId = 5, OrderId = 1, Quantity = 5, SubTotalPrice = 30});
            _ctx.OrderLines.Add(new OrderLine {ProductId = 6, OrderId = 1, Quantity = 1, SubTotalPrice = 6});
            _ctx.OrderLines.Add(new OrderLine {ProductId = 6, OrderId = 2, Quantity = 1, SubTotalPrice = 6});
            _ctx.OrderLines.Add(new OrderLine {ProductId = 1, OrderId = 3, Quantity = 1, SubTotalPrice = 18});
            _ctx.SaveChanges();
        }
    }
}