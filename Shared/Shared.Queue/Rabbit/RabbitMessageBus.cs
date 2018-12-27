using System;
using System.Threading.Tasks;
using EasyNetQ;
using Shared.Common;
using EasyNetQ.Consumer;
using EasyNetQ.FluentConfiguration;
using EasyNetQ.Producer;
using Shared.Common.Interfaces;

namespace Shared.Queue.Rabbit
{
    public class RabbitMessageBus : IMessageBus
    {
        public RabbitMessageBus(string connectionString)
        {
            Bus = RabbitHutch.CreateBus(connectionString);
        }

        private IBus Bus { get;  set; }

        public async Task PublishAsync<T>(T busEvent) where T : class, IBusEvent
        {
            if (Bus != null)
            {
                await Bus.PublishAsync(busEvent);
            }
            else
            {
                throw new ApplicationException("RabbitMqBus is not yet Initialized");
            }
        }

        public void Publish<T>(T busEvent) where T : class, IBusEvent
        {
            if (Bus != null)
            {
                Bus.Publish<T>(busEvent);
            }
            else
            {
                throw new ApplicationException("RabbitMqBus is not yet Initialized");
            }
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class, IBusEvent
        {
            var result = Bus.Subscribe(subscriptionId, onMessage);
        }

        //public async Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : BusEvent
        //{
        //    var result = await Bus.SubscribeAsync(subscriptionId, onMessage);
        //}
    }
}
