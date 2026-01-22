using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class PaymentCheckoutViewModel
    {
        [Required]
        public Guid CourseId { get; set; }
    }
}
