using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IGradedAttemptService
    {
        Task<ApiResponse> StartAttemptAsync(Guid gradedItemId);
        Task<ApiResponse> SubmitShortAnswerAsync(Guid attemptId, Guid questionId, string answer, IFormFile? file);
        Task<ApiResponse> SubmitAttemptAsync(Guid attemptId);
        Task<ApiResponse> GradeAssignmentAsync(Guid attemptId, decimal score);
    }
}
