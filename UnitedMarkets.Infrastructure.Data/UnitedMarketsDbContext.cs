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
        public DbSet<Origin> OriginCountries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.AmountUnit)
                .WithMany(unit => unit.Products);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Origin)
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

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(os => os.Orders);

            //modelBuilder.Entity<Order>().Property<bool>("IsDeleted");
            modelBuilder.Entity<Order>().HasQueryFilter(order => EF.Property<bool>(order, "IsDeleted") == false);
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
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
    }
}