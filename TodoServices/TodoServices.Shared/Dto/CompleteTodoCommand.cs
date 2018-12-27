using Shared.Common;
using System.Collections.Generic;

namespace TodoServices.Shared.Dto
{
    public class CompleteTodoCommand : DtoModel
    {
        public CompleteTodoCommand()
        {
        }

        public CompleteTodoCommand(int id, int performerId, decimal cost)
        {
            Id = id;
            Cost = cost;
            PerformerId = performerId;
        }


        public CompleteTodoCommand(int id, int performerId, decimal cost,int parentId)
        {
            Id = id;
            Cost = cost;
            PerformerId = performerId;
            ParentId = parentId;
            
            
        }
        public int Id { get; set; }

        public int PerformerId { get; set; }

        public decimal Cost { get; set; }

        public int ParentId { get; set; }
               
    }
}