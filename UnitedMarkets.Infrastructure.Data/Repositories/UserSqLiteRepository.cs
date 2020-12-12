using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<User> ReadAll()
        {
            return _ctx.Users.ToList();
        }

        public User ReadById(long id)
        {
            throw new NotImplementedException();
        }

        public User ReadByName(string username)
        {
            return _ctx.Users.ToList().FirstOrDefault(user => user.Username == username);
        }

        public User Create(User entity)
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}