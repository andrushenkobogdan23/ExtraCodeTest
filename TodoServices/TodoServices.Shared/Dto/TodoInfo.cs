using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Common;
using TodoServices.Domain.Model;

namespace TodoServices.Shared.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class TodoInfo : DtoModel
    {
        /// <summary>
        /// Ідентифікатор задачі
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ідентифікатор користувача, що створив задачу
        /// </summary>
        [DisplayName("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// Ідентифікатор користувача, що виконав задачу
        /// </summary>
        [DisplayName("Performer")]
        public int? PerformerId { get; set; }

        /// <summary>
        /// Оціночна вартість виконання задачі
        /// </summary>
        [DisplayName("Estimated cost"), Required(ErrorMessage = "Type estimated cost")]
        public decimal EstimatedCost { get; set; }

        [DisplayName("All Estimated cost"), Required(ErrorMessage = "Type estimated cost")]
        public decimal AllEstimatedCost { get; set; }

        /// <summary>
        /// Пріоритет задачі
        /// </summary>
        [Required(ErrorMessage = "Type priority")]
        public byte Priority { get; set; }

        /// <summary>
        /// Заголовок задачі
        /// </summary>
        [Required(ErrorMessage ="Type title"), MaxLength(32, ErrorMessage = "Title should be no more then 32 symbols") ]
        public string Title { get; set; }

        /// <summary>
        /// Опис задачі
        /// </summary>
        [Required(ErrorMessage = "Type description"), StringLength(32, ErrorMessage = "Description should be at least 5 symbols but no more then 32", MinimumLength = 5)]
        public string Description { get; set; }

        [DisplayName("Create date")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Complete date")]
        public DateTime? CompleteDate { get; set; }

        [DisplayName("Final cost")]
        public decimal? Cost { get; set; }

        [DisplayName("All Final cost")]
        public decimal? AllCost { get; set; }

        public int? ParentId { get; set; }

        public ICollection<TodoInfo> Childs { get; set; } 

    }
}