using Domain.Requests.Question;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IQuestionService
    {
        Task<ApiResponse> CreateQuestionAsync(CreateQuestionRequest request);
    }
}
