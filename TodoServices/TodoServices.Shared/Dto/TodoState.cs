namespace TodoServices.Shared.Dto
{
    public class TodoState
    {
        public TodoState(int id, string title) : this(id, title, false)
        {
        }

        public TodoState(int id, string title, bool state)
        {
            Id = id;
            Title = title;
            State = state;
        }

        public int Id { get; set; }
        public bool State { get; set; }
        public string Title { get; set; }
    }
}
