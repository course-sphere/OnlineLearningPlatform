using Domain.Responses;
using Domain.Entities;
using Domain.Entities;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IDashboardService
    {
        // ===== Cards =====
        Task<ApiResponse> GetTotalUsersAsync();
        Task<ApiResponse> GetTotalInstructorsAsync();
        Task<ApiResponse> GetTotalStudentsAsync();

        Task<ApiResponse> GetTotalCoursesAsync();
        Task<ApiResponse> GetActiveCoursesAsync();

        Task<ApiResponse> GetTotalEnrollmentsAsync();
        Task<ApiResponse> GetTotalRevenueAsync();

        // ===== Tables / Charts =====
        Task<ApiResponse> GetRecentActivitiesAsync(int take = 5);
        Task<ApiResponse> GetLatestEnrollmentsAsync(int take = 5);

        // ===== User Management =====
        Task<ApiResponse> GetAllUsersAsync();
        Task<ApiResponse> GetUserByIdAsync(Guid userId);
        Task<ApiResponse> GetUsersPaginatedAsync(int pageNumber, int pageSize, string searchTerm = null, Role? roleFilter = null);
        Task<ApiResponse> UpdateUserAsync(Guid userId, string fullName, string email, string phoneNumber, Role role);
        Task<ApiResponse> DeleteUserAsync(Guid userId);
    }
}