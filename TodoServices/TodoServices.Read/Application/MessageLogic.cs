using Newtonsoft.Json;
using Serilog;
using Shared.Common;
using Shared.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TodoServices.Domain;
using TodoServices.Shared.Dto;
using TodoServices.Shared.Events;

namespace TodoServices.Read.Application
{
    public class MessageLogic
    {
        public MessageLogic(SqlDbContext context, IMessageBus bus)
        {
            TodoStates = context.Todos.Select(
                    s => new TodoState(s.Id, s.Title, s.CompleteDate.HasValue)
                ).ToList();

            bus.Subscribe<TodoCompletedEvent>("todo.states", HandleMessage);
            bus.Subscribe<TodoAddedEvent>("todo.states", HandleMessage);
            bus.Subscribe<TodoDeletedEvent>("todo.states", HandleMessage);
            bus.Subscribe<TodoUpdatedEvent>("todo.states", HandleMessage);
        }

        internal IList<TodoState> TodoStates { get; private set; }

        internal void HandleMessage(TodoCompletedEvent msg)
        {
            LogEvent(msg);

            var item = TodoStates.FirstOrDefault(x => x.Id == msg.Id);
            item.State = true;
        }

        internal void HandleMessage(TodoUpdatedEvent msg)
        {
            LogEvent(msg);

            var item = TodoStates.FirstOrDefault(x => x.Id == msg.Id);
            item.Title = msg.Title;
        }

        internal void HandleMessage(TodoAddedEvent msg)
        {
            LogEvent(msg);

            TodoStates.Add(new TodoState(msg.Id, msg.Title));
        }

        internal void HandleMessage(TodoDeletedEvent msg)
        {
            LogEvent(msg);

            var item = TodoStates.FirstOrDefault(x => x.Id == msg.Id);
            TodoStates.Remove(item);
        }

        private void LogEvent(BusEvent msg)
        {
            Log.Debug("Handled event {0}: {1}", msg.GetType().Name, JsonConvert.SerializeObject(msg));
        }


        internal static MessageLogic Avatar { get; set; }
    }
}
