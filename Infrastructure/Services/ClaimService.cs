using Application.IServices;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ClaimService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public ClaimDTO GetUserClaim()
        {
            var user = _contextAccessor.HttpContext.User;
            if (user == null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            var tokenUserId = _contextAccessor.HttpContext!.User.FindFirst("UserId");
            var tokenUserRole = _contextAccessor.HttpContext!.User.FindFirst("Role");
            if (tokenUserId == null)
            {
                throw new ArgumentNullException("UserId can not be found!");
            }
            var userId = Guid.Parse(tokenUserId?.Value.ToString()!);
            var role = Enum.Parse<Role>(tokenUserRole?.Value.ToString()!);
            var userClaim = new ClaimDTO
            {
                Role = role,
                UserId = userId,
            };
            return userClaim;
        }
    }
}
