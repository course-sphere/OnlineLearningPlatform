using Domain.Requests.User;
using Domain.Responses;

namespace Application.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginRequest request);
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
    }
}
