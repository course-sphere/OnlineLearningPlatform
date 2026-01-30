using System;

namespace Domain.Responses
{
    public class LatestEnrollmentResult
    {
        public DateTime? EnrolledAt { get; set; }
        public string UserEmail { get; set; }
        public string CourseName { get; set; }
        public decimal Progress { get; set; }
        public string Status { get; set; }
    }
}