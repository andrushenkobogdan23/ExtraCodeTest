using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Common.Sql
{
    public class GenericRepository : IGenericRepository
    {
        protected DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public T GetById<T, K>(K id) where T : SimpleDomainModel
        {
            T type = _context.Set<T>().Find(id);
            return type;
        }

        public async Task<T> GetByIdAsync<T, K>(K id) where T : SimpleDomainModel
        {
            T type = await _context.Set<T>().FindAsync(id);
            return type;
        }

        public IQueryable<T> GetAll<T>() where T : SimpleDomainModel
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        public Task<IQueryable<T>> GetAllAsync<T>() where T : SimpleDomainModel
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(T entity) where T : SimpleDomainModel
        {
            _context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : SimpleDomainModel
        {
            _context.Set<T>().Attach(entity);
        }

        public void Delete<T>(T entity) where T : SimpleDomainModel
        {
            _context.Set<T>().Remove(entity);
        }

        public DbContext Context { get { return _context; } }
    }
}
