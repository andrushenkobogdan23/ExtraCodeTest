using Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoServices.Domain.Model
{
    /// <summary>
    /// робота
    /// </summary>
    public class Todo : TypedDomainModel<int>
    {
        /// <summary>
        /// Дата створення
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата виконання
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// Ідентифікатор користувача, що створив задачу
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Ідентифікатор користувача, що виконав задачу
        /// </summary>
        public int? PerformerId { get; set; }

        /// <summary>
        /// Оціночна вартість виконання задачі
        /// </summary>
        public decimal EstimatedCost { get; set; }

        /// <summary>
        /// Фактична вартість виконання задачі
        /// </summary>
        public decimal? Cost { get; set; }

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

       // public decimal? ChildCost { get; set; }

        public Todo Parent { get; set; }

        [ForeignKey(nameof(Todo))]
        public int? ParentId { get; set; }

        public virtual ICollection<Todo> Childs { get; set; }
    }
}
