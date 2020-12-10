using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class UserSqLiteRepository : IRepository<User>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public UserSqLiteRepository(UnitedMarketsDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<User> GetAll()
        {
            return _ctx.Users.ToList();
        }

        public User Get(long id)
        {
            return _ctx.Users.FirstOrDefault(user => user.Id == id);
        }

        public User GetByName(string username)
        {
            return _ctx.Users.ToList().FirstOrDefault(user => user.Username == username);
        }

        public void Create(User entity)
        {
            _ctx.Users.Add(entity);
            _ctx.SaveChanges();
        }

        public void Update(User entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void Delete(long id)
        {
            var item = _ctx.Users.FirstOrDefault(b => b.Id == id);
            if (item != null) _ctx.Users.Remove(item);
            _ctx.SaveChanges();
        }
    }
}