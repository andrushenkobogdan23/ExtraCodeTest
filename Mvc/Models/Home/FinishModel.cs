using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Home
{
    public class FinishModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type final cost")]
        public decimal Cost { get; set; }
    }
}