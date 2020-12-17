using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class StatusService : IService<Status>
    {
        private readonly IRepository<Status> _statusRepository;

        public StatusService(IRepository<Status> statusRepository)
        {
            _statusRepository = statusRepository ??
                                throw new ArgumentNullException(nameof(statusRepository),
                                    "Repository cannot be null.");
        }

        public Status Create(Status entity)
        {
            throw new NotImplementedException();
        }

        public List<Status> GetAll()
        {
            return _statusRepository.ReadAll().ToList();
        }

        public FilteredList<Status> GetAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Status Update(Status entity)
        {
            throw new NotImplementedException();
        }

        public Status Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}