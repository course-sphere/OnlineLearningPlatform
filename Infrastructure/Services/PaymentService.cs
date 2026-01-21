using Application;
using Application.IServices;
using Domain.Requests.Payment;
using Domain.Responses;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _service;

        public PaymentService(IConfiguration config, IUnitOfWork unitOfWork, IClaimService service)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewPayment(CreateNewPaymentRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
