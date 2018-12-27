using System;
using System.Threading.Tasks;

namespace Shared.Common.Interfaces
{
    public interface IMessageBus
    {
        void Publish<T>(T busEvent) where T : class, IBusEvent;

        Task PublishAsync<T>(T busEvent) where T : class, IBusEvent;

        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class, IBusEvent;

        //Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : BusEvent;
    }
}
