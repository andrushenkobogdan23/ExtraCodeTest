using Shared.Common;
using Shared.Common.Interfaces;
using System.Collections.Generic;
using TodoServices.Domain.Model;
using TodoServices.Shared.Dto;

namespace TodoServices.Command.Commands
{
    internal class TodoAddCommand: ICommand, ITodoCheckFields
    {
        internal TodoAddCommand(int authorId, string title, string description, decimal estimatedCost, byte priority,int?  parentId)
        {
            AuthorId = authorId;
            Title = title;
            Description = description;
            EstimatedCost = estimatedCost;
            Priority = priority;
            ParentId = parentId;
                    }

        internal TodoAddCommand(int authorId, string title, string description, decimal estimatedCost, byte priority)
        {
            AuthorId = authorId;
            Title = title;
            Description = description;
            EstimatedCost = estimatedCost;
            Priority = priority;
            
        }

        /// <summary>
        /// ідентифікатор задачі, встановлюється після збереження
        /// </summary>
        internal int Id { get; set; }

        public int AuthorId { get; set; }

        public decimal EstimatedCost { get; set; }

        public byte Priority { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }
           }

    public interface ITodoCheckFields
    {
        /// <summary>
        /// Ідентифікатор користувача, що створив задачу
        /// </summary>
        int AuthorId { get; set; }

        /// <summary>
        /// Оціночна вартість виконання задачі
        /// </summary>
        decimal EstimatedCost { get; set; }

        /// <summary>
        /// Пріоритет задачі
        /// </summary>
        byte Priority { get; set; }

        /// <summary>
        /// Заголовок задачі
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Опис задачі
        /// </summary>
        string Description { get; set; }

        int? ParentId { get; set; }
       
    }
}