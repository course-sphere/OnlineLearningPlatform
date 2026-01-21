using Domain.Requests.Payment;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface IPaymentService
    {
        Task<ApiResponse> CreatePaymentUrlAsync(CreateNewPaymentRequest request, HttpContext context);
        Task<ApiResponse> PaymentExecuteAsync(IQueryCollection collection);
    }
}
