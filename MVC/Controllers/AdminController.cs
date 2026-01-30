using Application.IServices;
using Domain.Responses;
using Domain.Entities;
using Domain.Requests.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Dashboard;
using MVC.Models.UserManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICourseService _courseService;

        public AdminController(IDashboardService dashboardService, ICourseService courseService)
        {
            _dashboardService = dashboardService;
            _courseService = courseService;
        }

        // https://localhost:7276/Admin - redirect to Dashboard
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        // ===== DASHBOARD =====
        public async Task<IActionResult> Dashboard()
        {
            var vm = new DashboardViewModel();

            var totalUsers = await _dashboardService.GetTotalUsersAsync();
            var totalCourses = await _dashboardService.GetTotalCoursesAsync();
            var activeCourses = await _dashboardService.GetActiveCoursesAsync();
            var totalEnrollments = await _dashboardService.GetTotalEnrollmentsAsync();
            var totalRevenue = await _dashboardService.GetTotalRevenueAsync();
            var enrollmentsRes = await _dashboardService.GetLatestEnrollmentsAsync();

            if (totalUsers.IsSuccess) vm.TotalUsers = (int)totalUsers.Result;
            if (totalCourses.IsSuccess) vm.TotalCourses = (int)totalCourses.Result;
            if (activeCourses.IsSuccess) vm.ActiveCourses = (int)activeCourses.Result;
            if (totalEnrollments.IsSuccess) vm.TotalEnrollments = (int)totalEnrollments.Result;

            vm.TotalRevenue = totalRevenue.IsSuccess && totalRevenue.Result != null
                ? Convert.ToDecimal(totalRevenue.Result)
                : 0;

            if (enrollmentsRes.IsSuccess)
            {
                var data = enrollmentsRes.Result as List<LatestEnrollmentResult>;
                if (data != null)
                {
                    vm.LatestEnrollments = data.Select(e => new LatestEnrollmentVM
                    {
                        EnrolledAt = e.EnrolledAt,
                        UserEmail = e.UserEmail,
                        CourseName = e.CourseName,
                        Progress = e.Progress,
                        Status = e.Status
                    }).ToList();
                }
            }

            return View(vm);
        }

        // ===== USER MANAGEMENT =====
        public async Task<IActionResult> Users(
            int pageNumber = 1,
            int pageSize = 10,
            string searchTerm = null,
            Role? roleFilter = null)
        {
            var response = await _dashboardService
                .GetUsersPaginatedAsync(pageNumber, pageSize, searchTerm, roleFilter);

            var vm = new UserManagementViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                RoleFilter = roleFilter
            };

            if (!response.IsSuccess || response.Result == null)
            {
                TempData["ErrorMessage"] = response.ErrorMessage ?? "Failed to load users";
                return View(vm);
            }

            var result = response.Result as PagedUsersResult;

            if (result == null)
            {
                TempData["ErrorMessage"] = "Invalid users data format";
                return View(vm);
            }

            var users = result.Users;

            if (users != null)
            {
                vm.Users = users.Select(u => new UserViewModel
                {
                    UserId = u.UserId,
                    FullName = u.FullName ?? "",
                    Email = u.Email ?? "",
                    PhoneNumber = u.PhoneNumber ?? "",
                    Role = u.Role,
                    IsVerfied = u.IsVerfied,
                    Image = u.Image,
                    CreatedAt = u.CreatedAt
                }).ToList();
            }

            vm.TotalCount = result.TotalCount;
            vm.TotalPages = result.TotalPages;

            return View(vm);
        }

        // GET: /Admin/EditUser/{id}
        public async Task<IActionResult> EditUser(Guid id)
        {
            var response = await _dashboardService.GetUserByIdAsync(id);

            if (!response.IsSuccess || response.Result == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction(nameof(Users));
            }

            return View(response.Result);
        }

        // POST: /Admin/UpdateUser
        [HttpPost]
        public async Task<IActionResult> UpdateUser(Guid userId, string fullName, string email, string phoneNumber, Role role)
        {
            var response = await _dashboardService.UpdateUserAsync(userId, fullName, email, phoneNumber, role);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "User updated successfully";
                return RedirectToAction(nameof(Users));
            }
            else
            {
                TempData["ErrorMessage"] = response.ErrorMessage ?? "Update user failed";
                return RedirectToAction(nameof(EditUser), new { id = userId });
            }
        }

        // POST: /Admin/DeleteUser
        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var response = await _dashboardService.DeleteUserAsync(userId);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "User deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = response.ErrorMessage ?? "Delete user failed";
            }

            return RedirectToAction(nameof(Users));
        }

        // ===== COURSE MANAGEMENT =====
        // https://localhost:7276/Admin/PendingCourses
        [HttpGet]
        public async Task<IActionResult> PendingCourses()
        {
            var result = await _courseService.GetAllCourseForAdminAsync(CourseStatus.PendingApproval);
            return View(result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid courseId)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = true,
                RejectReason = null
            };

            var result = await _courseService.ApproveCourseAsync(request);

            return RedirectToAction("PendingCourses");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid courseId, string rejectReason)
        {
            var request = new ApproveCourseRequest
            {
                CourseId = courseId,
                Status = false,
                RejectReason = rejectReason
            };

            var result = await _courseService.ApproveCourseAsync(request);

            return RedirectToAction("PendingCourses");
        }

        [HttpGet]
        public async Task<IActionResult> PreviewCourse(Guid id)
        {
            var result = await _courseService.GetCourseLearningDetailAsync(id);
            if (!result.IsSuccess) return RedirectToAction("PendingCourses");

            return View(result.Result);
        }

        // ===== OTHER PAGES =====
        public IActionResult CourseStatistics()
        {
            return View();
        }

        public IActionResult Payments()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }
    }
}