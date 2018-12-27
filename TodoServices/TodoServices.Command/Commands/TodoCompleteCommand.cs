using Shared.Common;
using Shared.Common.Interfaces;
using System.Collections.Generic;
using TodoServices.Shared.Dto;

namespace TodoServices.Command.Commands
{
    internal class TodoCompleteCommand: ICommand
    {
        internal TodoCompleteCommand()
        {

        }

        internal TodoCompleteCommand(int id, int performerId, decimal cost)
        {
            Id = id;
            Cost = cost;
            PerformerId = performerId;
        }

        internal TodoCompleteCommand(int id, int performerId, decimal cost, int parentId)
        {
            Id = id;
            Cost = cost;
            PerformerId = performerId;
            ParentId = parentId;                  
        }

        internal int Id { get; private set; }
        internal int PerformerId { get; private set; }
        internal decimal Cost { get; set; }
        public int? ParentId { get; set; }            

    }
}