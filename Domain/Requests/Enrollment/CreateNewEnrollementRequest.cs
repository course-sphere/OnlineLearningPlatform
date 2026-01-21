using Domain.Entities;

namespace Domain.Requests.Enrollment
{
    public class CreateNewEnrollementRequest
    {
        public Guid CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
