using Shared.Common;

namespace TodoServices.Shared.Events
{
    public class TodoCompletedEvent : TypedBusEvent<int>
    {

        public TodoCompletedEvent(int id, string title, int authorId, decimal cost)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
            Cost = cost;
        }
        
        public TodoCompletedEvent(int id, string title, int authorId, decimal cost,int? parentId)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
            Cost = cost;
            ParentId = parentId;
        }

        public string Title { get; set; }
        public int AuthorId { get; private set; }
        public decimal Cost { get; set; }
        public int? ParentId { get; set; }

    }
}
