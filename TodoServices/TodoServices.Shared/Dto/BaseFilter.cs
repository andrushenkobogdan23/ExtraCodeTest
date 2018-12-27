using System;
using System.ComponentModel;

namespace TodoServices.Shared.Dto
{
    public class BaseFilter
    {
        public BaseFilter()
        {
            var today = DateTime.Today;
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
            To = today.AddDays(daysUntilFriday);
            From = To.AddDays(-15);
        }

        /// <summary>
        /// ідентифікатор виконавця
        /// </summary>
        [DisplayName("Completed by")]
        public int PerformerId { get; set; }

        /// <summary>
        /// дата з
        /// </summary>
        [DisplayName("Create date from")]
        public DateTime From { get; set; }

        /// <summary>
        /// дата по
        /// </summary>
        [DisplayName("Created date to")]
        public DateTime To { get; set; }

        public bool OnlyParent { get; set; }
    }
}