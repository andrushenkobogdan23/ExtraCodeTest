using System.Threading.Tasks;

namespace Shared.Common.Sql
{
    public interface IUnitOfWork
    {
        IGenericRepository Repository { get; }

        void Save();

        Task SaveAsync();

        Task PublishEventAsync<M>(M msg) where M : BusEvent;

        Task SaveAndPublish<M>(M msg) where M : BusEvent;
    }
}
