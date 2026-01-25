using Application;
using Application.IServices;
using AutoMapper;
using Domain.Requests.Question;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _service;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _service = service;
        }

        public async Task<ApiResponse> CreateQuestionAsync(CreateQuestionRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var question = _mapper.Map<Domain.Entities.Question>(request);
                return response.SetOk(question);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
