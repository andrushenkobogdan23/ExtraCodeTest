using Shared.Common;

namespace TodoServices.Shared.Events
{
    public class TodoUpdatedEvent : TypedBusEvent<int>
    {

        public TodoUpdatedEvent(int id, int oldAuthorId, int newAuthorId, decimal estimatedCost, string title)
        {
            Id = id;
            OldAuthorId = oldAuthorId;
            NewAuthorId = NewAuthorId;
            EstimatedCost = estimatedCost;
            Title = title;
        }
        public TodoUpdatedEvent(int id, int oldAuthorId, int newAuthorId, decimal estimatedCost, string title,int?parentId)
        {
            Id = id;
            OldAuthorId = oldAuthorId;
            NewAuthorId = NewAuthorId;
            EstimatedCost = estimatedCost;
            Title = title;
            ParentId = parentId;


        }

        public int OldAuthorId { get; set; }

        public int NewAuthorId { get; set; }

        public decimal EstimatedCost { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }
    }
}
