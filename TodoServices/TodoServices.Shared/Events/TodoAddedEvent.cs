using Shared.Common;

namespace TodoServices.Shared.Events
{
    public class TodoAddedEvent : TypedBusEvent<int>
    {
        public TodoAddedEvent(int id, int authorId, string title)
        {
            Id = id;
            AuthorId = authorId;
            Title = title;
        }

        public int AuthorId { get; set; }

        public string Title { get; set; }
    }
}
