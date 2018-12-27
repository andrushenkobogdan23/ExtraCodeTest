using Newtonsoft.Json;
using Serilog;
using SerilogTimings;
using Shared.Common;
using Shared.Common.Interfaces;
using TodoServices.Shared.Dto;
using TodoServices.Shared.Events;

namespace TodoServices.Message.Application
{
    public class MessageLogic
    {
        private IMessageBus bus;

        public MessageLogic(IMessageBus bus)
        {
            this.bus = bus;
            bus.Subscribe<TodoCompletedEvent>("todo.msgs", HandleMessage);
            bus.Subscribe<TodoAddedEvent>("todo.msgs", HandleMessage);
            bus.Subscribe<TodoDeletedEvent>("todo.msgs", HandleMessage);
            bus.Subscribe<TodoUpdatedEvent>("todo.msgs", HandleMessage);
        }


        internal void HandleMessage(TodoCompletedEvent msg)
        {
            LogEvent(msg);

            var fakeMessage = new TodoMessage
            {
                Email = "me@mail.com",
            };
            fakeMessage.Subject = $"todo with id='{msg.Id}' has been completed by authorid='{msg.AuthorId}'";
            fakeMessage.Message = $"{fakeMessage.Subject}\nCost='{msg.Cost}'\nTitle='{msg.Title}'\nTodoId='{msg.Id}'";

            Send(fakeMessage);
        }

        internal void HandleMessage(TodoUpdatedEvent msg)
        {
            LogEvent(msg);

            var fakeMessage = new TodoMessage
            {
                Email = "me@mail.com",
            };

            if (msg.NewAuthorId != msg.OldAuthorId)
            {
                fakeMessage.Subject = $"your todo with id='{msg.Id}' has been changed by authorid='{msg.NewAuthorId}'";
                fakeMessage.Message = $"{fakeMessage.Subject}\nTitle='{msg.Title}'\nTodoId='{msg.Id}'";
                Send(fakeMessage);
            }
        }

        internal void HandleMessage(TodoAddedEvent msg)
        {
            LogEvent(msg);

            var fakeMessage = new TodoMessage
            {
                Email = "me@mail.com",
            };

            fakeMessage.Subject = $"new todo: {msg.Title}";
            fakeMessage.Message = $"{fakeMessage.Subject}\nAuthorId='{msg.AuthorId}'\nTodoId='{msg.Id}'";
            Send(fakeMessage);
        }

        internal void HandleMessage(TodoDeletedEvent msg)
        {
            LogEvent(msg);

            var fakeMessage = new TodoMessage
            {
                Email = "me@mail.com",
            };

            fakeMessage.Subject = $"todo with id='{msg.Id}' has been deleted";
            Send(fakeMessage);
        }

        private void LogEvent(BusEvent msg)
        {
            Log.Debug("Handled event {0}: {1}", msg.GetType().Name, JsonConvert.SerializeObject(msg));
        }


        internal static MessageLogic Avatar { get; set; }


        internal void Send(TodoMessage msg)
        {
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("got message: {0}", JsonConvert.SerializeObject(msg)))
            {
                // some message logic
                op.Complete();
            }
        }
    }
}
