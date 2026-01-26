using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class PaymentCheckoutViewModel
    {
        [Required]
        public Guid CourseId { get; set; }

        public string? CourseName { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? InstructorName { get; set; }
    }
}
