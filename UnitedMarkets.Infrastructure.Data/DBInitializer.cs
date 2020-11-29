using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UnitedMarketsDbContext _ctx;
        private readonly IMarketRepository _marketRepository;

        public DbInitializer(UnitedMarketsDbContext ctx, IMarketRepository marketRepository)
        {
            _ctx = ctx;
            _marketRepository = marketRepository;
        }

        public void InitData()
        {
            InitMarkets();
        }

        private void InitMarkets()
        {
            _marketRepository.Create(new Market() { Name = "Fakta" });
            _marketRepository.Create(new Market() { Name = "Irma" });
            _marketRepository.Create(new Market() { Name = "Netto" });
            _ctx.SaveChanges();
        }
    }
}
