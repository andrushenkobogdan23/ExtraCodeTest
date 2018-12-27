using Microsoft.EntityFrameworkCore;
using Shared.Common.Interfaces;
using System.Threading.Tasks;

namespace Shared.Common.Sql
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly DbContext _context;
        private readonly IMessageBus _bus;

        private IGenericRepository _repo;

        public UnitOfWork(T context, IMessageBus bus) 
        {
            _context = context;
            _bus = bus;
            //_context.Configuration.LazyLoadingEnabled = false;  
        }

        public IGenericRepository Repository
        {
            get
            {
                if (_repo == null)
                    _repo = new GenericRepository(_context);

                return _repo;
            }
        }

        public async Task PublishEventAsync<M>(M msg) where M : BusEvent
        {
            //await Task.FromResult(0);
            await _bus.PublishAsync(msg);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAndPublish<M>(M msg) where M : BusEvent
        {
            await SaveAsync();
            await PublishEventAsync(msg);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
