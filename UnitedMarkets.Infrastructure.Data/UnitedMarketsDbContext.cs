using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Infrastructure.Data
{
    public class UnitedMarketsDbContext : DbContext
    {
        public UnitedMarketsDbContext(DbContextOptions<UnitedMarketsDbContext> opt) : base(opt)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<AmountUnit> AmountUnits { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    Specify mapping of properties (not nullable, unique constraint, exclude)
            modelBuilder.Entity<User>()
                .Property(user => user.Username).IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.PasswordHash).IsRequired();
            modelBuilder.Entity<User>()
                .Property(user => user.PasswordSalt).IsRequired();
            modelBuilder.Entity<User>()
                .HasAlternateKey(user => user.Username);

            modelBuilder.Entity<AmountUnit>()
                .Property(amountUnit => amountUnit.Name).IsRequired();
            modelBuilder.Entity<AmountUnit>()
                .HasAlternateKey(amountUnit => amountUnit.Name);

            modelBuilder.Entity<Category>()
                .Property(category => category.Name).IsRequired();
            modelBuilder.Entity<Category>()
                .HasAlternateKey(category => category.Name);

            modelBuilder.Entity<Market>()
                .Property(market => market.Name).IsRequired();
            modelBuilder.Entity<Market>()
                .HasAlternateKey(market => market.Name);

            modelBuilder.Entity<Order>()
                .Property(order => order.BillingAddress).IsRequired();
            modelBuilder.Entity<Order>()
                .Property(order => order.ShippingAddress).IsRequired();
            /* Should use shadow properties instead to record the date of updates 
            modelBuilder.Entity<Order>()
                .Property<DateTime>("DateUpdated");
            */

            modelBuilder.Entity<Origin>()
                .Property(origin => origin.Name).IsRequired();
            modelBuilder.Entity<Origin>()
                .HasAlternateKey(origin => origin.Name);

            modelBuilder.Entity<Product>()
                .Property(product => product.Name).IsRequired();
            modelBuilder.Entity<Product>()
                .Ignore(product => product.Price);

            modelBuilder.Entity<Status>()
                .Property(status => status.Name).IsRequired();
            modelBuilder.Entity<Status>()
                .HasAlternateKey(status => status.Name);

            //    Relations
            modelBuilder.Entity<Product>()
                .HasOne(product => product.AmountUnit)
                .WithMany(amountUnit => amountUnit.Products);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Origin)
                .WithMany(origin => origin.Products);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Market)
                .WithMany(market => market.Products);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Status)
                .WithMany(status => status.Orders);

            modelBuilder.Entity<OrderLine>()
                .HasKey(orderLine => new {orderLine.OrderId, orderLine.ProductId});

            modelBuilder.Entity<OrderLine>()
                .HasOne(orderLine => orderLine.Order)
                .WithMany(order => order.Products)
                .HasForeignKey(orderLine => new {orderLine.OrderId});

            modelBuilder.Entity<OrderLine>()
                .HasOne(orderLine => orderLine.Product)
                .WithMany(product => product.Orders)
                .HasForeignKey(orderLine => new {orderLine.ProductId});

            //    Soft delete query filter
            //modelBuilder.Entity<Order>().Property<bool>("IsDeleted");
            modelBuilder.Entity<Order>().HasQueryFilter(order => EF.Property<bool>(order, "IsDeleted") == false);
            modelBuilder.Entity<OrderLine>()
                .HasQueryFilter(orderLine => EF.Property<bool>(orderLine.Order, "IsDeleted") == false);
        }

        public override int SaveChanges()
        {
            //ModifyDateUpdated();
            UpdateSoftDeleteStatus();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatus();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatus()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
                if (item.Entity is ISoftDelete entity)
                {
                    // Set the entity to unchanged (Modified will update every field)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag
                    entity.IsDeleted = true;
                }
        }

        private void ModifyDateUpdated()
        {
            ChangeTracker.DetectChanges();

            var orders = ChangeTracker.Entries().Where(x => x.GetType() == typeof(Order));
            foreach (var entry in orders)
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    entry.Property("DateUpdated").CurrentValue = DateTime.UtcNow;
        }
    }
}