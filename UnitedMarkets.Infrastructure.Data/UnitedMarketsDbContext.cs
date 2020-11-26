﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data
{
    public class UnitedMarketsDbContext : DbContext
    {
        public UnitedMarketsDbContext(DbContextOptions<UnitedMarketsDbContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.AmountUnit)
                .WithMany(unit => unit.Products);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.OriginCountry)
                .WithMany(m => m.Products);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Market)
                .WithMany(m => m.Products);


            modelBuilder.Entity<OrderLine>()
                .HasKey(ol => new {ol.OrderId, ol.ProductId});

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(ol => new {ol.OrderId});

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(ol => new {ol.ProductId});
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<AmountUnit> AmountUnits { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> Orderlines { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OriginCountry> OriginCountries { get; set; }
    }
}