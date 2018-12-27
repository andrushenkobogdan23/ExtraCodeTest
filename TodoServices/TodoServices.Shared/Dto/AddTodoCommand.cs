
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Common;

namespace TodoServices.Shared.Dto
{
    public class AddTodoCommand : DtoModel
    {
        /// <summary>
        /// Ідентифікатор користувача, що створив задачу
        /// </summary>
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
        [Required(ErrorMessage = "Type title"), MaxLength(32, ErrorMessage = "Title should be no more then 32 symbols")]
        public string Title { get; set; }

        /// <summary>
        /// Опис задачі
        /// </summary>
        [Required(ErrorMessage = "Type description"), StringLength(32, ErrorMessage = "Description should be at least 5 symbols but no more then 32", MinimumLength = 5)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

    }
}