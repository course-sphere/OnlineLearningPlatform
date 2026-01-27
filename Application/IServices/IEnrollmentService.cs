using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Responses;
using Domain.Requests.Enrollment;

namespace Application.IServices
{
    public interface IEnrollmentService
    {
        Task<ApiResponse> EnrollStudentDirectlyAsync(Guid courseId);
        Task<ApiResponse> GetStudentEnrollmentsAsync();
    }
}
