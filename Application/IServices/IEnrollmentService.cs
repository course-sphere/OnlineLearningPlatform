using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Responses;

namespace Application.IServices
{
    public interface IEnrollmentService
    {
        Task<ApiResponse> EnrollStudentAsync(Guid courseId);
        Task<ApiResponse> GetStudentEnrollmentsAsync();
    }
}