using Domain.Entities;

namespace Domain.Requests.Payment
{
    public class CreateNewPaymentRequest
    {
        public Guid CourseId { get; set; }
        public PaymentMethod Method { get; set; }
    }
}
