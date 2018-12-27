using Shared.Common;
using System.Collections.Generic;

namespace TodoServices.Shared.Dto
{
    public class UpdateTodoCommand : DtoModel
    {
        /// <summary>
        /// Ідентифікатор задачі
        /// </summary>
        public int Id { get; set; }

        public int AuthorId { get; set; }

        /// <summary>
        /// Оціночна вартість виконання задачі
        /// </summary>
        public decimal EstimatedCost { get; set; }

        /// <summary>
        /// Пріоритет задачі
        /// </summary>
        public byte Priority { get; set; }

        /// <summary>
        /// Заголовок задачі
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Опис задачі
        /// </summary>
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public decimal? ChildCost { get; set; }

        public List<TodoInfo> Childs { get; set; }
    }

}
