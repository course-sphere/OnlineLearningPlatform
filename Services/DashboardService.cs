using Application;
using Application.IServices;
using Domain.Responses;
using Domain;
using Domain.Entities;
using Domain.Entities;
using Domain.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===== DASHBOARD METHODS =====
        public async Task<ApiResponse> GetTotalUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return new ApiResponse().SetOk(users.Count());
        }

        public async Task<ApiResponse> GetTotalInstructorsAsync()
        {
            var instructors = await _unitOfWork.Users
                .GetAllAsync(u => u.Role == Role.Instructor);

            return new ApiResponse().SetOk(instructors.Count());
        }

        public async Task<ApiResponse> GetTotalStudentsAsync()
        {
            var students = await _unitOfWork.Users
                .GetAllAsync(u => u.Role == Role.Student);

            return new ApiResponse().SetOk(students.Count());
        }

        public async Task<ApiResponse> GetTotalCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();
            return new ApiResponse().SetOk(courses.Count());
        }

        public async Task<ApiResponse> GetActiveCoursesAsync()
        {
            var activeCourses = await _unitOfWork.Courses
                .GetAllAsync(c => c.IsDeleted == false);

            return new ApiResponse().SetOk(activeCourses.Count());
        }

        public async Task<ApiResponse> GetTotalEnrollmentsAsync()
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
            return new ApiResponse().SetOk(enrollments.Count());
        }

        public async Task<ApiResponse> GetTotalRevenueAsync()
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
            var courses = await _unitOfWork.Courses.GetAllAsync();

            var totalRevenue =
                (from e in enrollments
                 join c in courses on e.CourseId equals c.CourseId
                 where c.IsDeleted == false
                 select c.Price)
                .Sum();

            return new ApiResponse().SetOk(totalRevenue);
        }

        public async Task<ApiResponse> GetRecentActivitiesAsync(int take = 5)
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();

            var recent = enrollments
                .OrderByDescending(e => e.CreatedAt)
                .Take(take)
                .Select(e => new
                {
                    Time = e.CreatedAt,
                    UserId = e.UserId,
                    CourseId = e.CourseId,
                    Status = "Success"
                });

            return new ApiResponse().SetOk(recent);
        }
        public async Task<ApiResponse> GetLatestEnrollmentsAsync(int take = 5)
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var courses = await _unitOfWork.Courses.GetAllAsync();

            var data = (
                from e in enrollments
                join u in users on e.UserId equals u.UserId
                join c in courses on e.CourseId equals c.CourseId
                orderby e.EnrolledAt descending
                select new LatestEnrollmentResult
                {
                    EnrolledAt = e.EnrolledAt,
                    UserEmail = u.Email,
                    CourseName = c.Title,
                    Progress = e.ProgressPercent,
                    Status = e.Status.ToString()
                }
            )
            .Take(take)
            .ToList();

            return new ApiResponse().SetOk(data);
        }

        // ===== USER MANAGEMENT METHODS =====
        public async Task<ApiResponse> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync(u => u.IsDeleted == false);

            var userList = users.Select(u => new
            {
                u.UserId,
                u.FullName,
                u.Email,
                u.PhoneNumber,
                u.Role,
                u.IsVerfied,
                u.Image,
                u.CreatedAt
            }).ToList();

            return new ApiResponse().SetOk(userList);
        }

        public async Task<ApiResponse> GetUserByIdAsync(Guid userId)
        {
            var users = await _unitOfWork.Users.GetAllAsync(u => u.UserId == userId);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            return new ApiResponse().SetOk(user);
        }

        public async Task<ApiResponse> GetUsersPaginatedAsync(
    int pageNumber,
    int pageSize,
    string searchTerm = null,
    Role? roleFilter = null)
        {
            var users = await _unitOfWork.Users.GetAllAsync(u => u.IsDeleted == false);
            var query = users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u =>
                    u.FullName.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm));
            }

            if (roleFilter.HasValue)
            {
                query = query.Where(u => u.Role == roleFilter.Value);
            }

            var totalCount = query.Count();

            var pagedUsers = query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // 👈 trả ENTITY

            return new ApiResponse().SetOk(new PagedUsersResult
            {
                Users = pagedUsers,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        public async Task<ApiResponse> UpdateUserAsync(Guid userId, string fullName, string email, string phoneNumber, Role role)
        {
            var users = await _unitOfWork.Users.GetAllAsync(u => u.UserId == userId);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            user.FullName = fullName;
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangeAsync();
            return new ApiResponse().SetOk("User updated successfully");
        }

        public async Task<ApiResponse> DeleteUserAsync(Guid userId)
        {
            var users = await _unitOfWork.Users.GetAllAsync(u => u.UserId == userId);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new ApiResponse().SetNotFound("User not found");
            }

            // Soft delete
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk("User deleted successfully");
        }
    }
}