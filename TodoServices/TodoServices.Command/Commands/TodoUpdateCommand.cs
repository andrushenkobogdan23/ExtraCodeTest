using Shared.Common;
using Shared.Common.Interfaces;
using System.Collections.Generic;
using TodoServices.Shared.Dto;

namespace TodoServices.Command.Commands
{
    internal class TodoUpdateCommand : ICommand, ITodoCheckFields
    {
        internal TodoUpdateCommand(int id, int authorId, string title, string description, decimal estimatedCost, byte priority,int? parentId)
        {
            Id = id;

            Title = title;
            Description = description;
            EstimatedCost = estimatedCost;
            Priority = priority;
            AuthorId = authorId;
            ParentId = parentId;          
            
        }


        internal TodoUpdateCommand(int id, int authorId, string title, string description, decimal estimatedCost, byte priority)
        {
            Id = id;

            Title = title;
            Description = description;
            EstimatedCost = estimatedCost;
            Priority = priority;
            AuthorId = authorId;
            
        }

        internal int Id { get; set; }

        public int AuthorId { get; set; }
        public string Title { get;  set; }
        public string Description { get;  set; }
        public decimal EstimatedCost { get;  set; }
        public byte Priority { get;  set; }
        public int? ParentId { get; set; }
    }
}