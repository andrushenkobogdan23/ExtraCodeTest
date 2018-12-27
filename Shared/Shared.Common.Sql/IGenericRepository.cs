using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Common.Sql
{
    public interface IGenericRepository
    {
        DbContext Context { get; }

        T GetById<T, K>(K id) where T : SimpleDomainModel;
        Task<T> GetByIdAsync<T, K>(K id) where T : SimpleDomainModel;

        IQueryable<T> GetAll<T>() where T : SimpleDomainModel;
        Task<IQueryable<T>> GetAllAsync<T>() where T : SimpleDomainModel;

        void Insert<T>(T entity) where T : SimpleDomainModel;
        void Update<T>(T entity) where T : SimpleDomainModel;
        void Delete<T>(T entity) where T : SimpleDomainModel;
    }
}