using Shared.Common;

namespace TodoServices.Shared.Events
{
    public class TodoDeletedEvent : TypedBusEvent<int>
    {

        public TodoDeletedEvent(int id, int userId)
        {
            Id = id;
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
