using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<ApiResponse> EnrollStudentAsync(Guid courseId)
        {
            var response = new ApiResponse();
            try
            {
                // 1. Lấy User hiện tại
                var userId = _claimService.GetUserClaim().UserId;

                // 2. Check xem đã Enroll chưa (Tránh trùng)
                var existing = await _unitOfWork.Enrollments.GetAsync(e => e.CourseId == courseId && e.UserId == userId);
                if (existing != null)
                {
                    // Nếu đã enroll rồi thì trả về OK luôn để redirect vào học
                    return response.SetOk(existing.EnrollmentId);
                }

                // 3. Lấy thông tin khóa học để check giá
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == courseId);
                if (course == null) return response.SetNotFound("Course not found");

                // 4. LOGIC PHÂN LUỒNG: FREE vs PAID
                if (course.Price > 0)
                {
                    // === FLOW TRẢ PHÍ ===
                    // Kiểm tra xem đã có Payment thành công chưa
                    var successPayment = await _unitOfWork.Payments.GetAsync(p => p.CourseId == courseId && p.UserId == userId && p.IsSuccess);

                    if (successPayment == null)
                    {
                        // Nếu chưa thanh toán -> Báo lỗi để Controller điều hướng sang trang Checkout
                        return response.SetBadRequest("Payment required");
                    }
                }

                // 5. TẠO ENROLLMENT (Cho cả Free hoặc Paid đã thanh toán)
                var newEnrollment = new Enrollment
                {
                    EnrollmentId = Guid.NewGuid(),
                    CourseId = courseId,
                    UserId = userId,
                    EnrolledAt = DateTime.UtcNow,
                    Status = EnrollmentStatus.Active,
                    ProgressPercent = 0
                };

                await _unitOfWork.Enrollments.AddAsync(newEnrollment);

                // 6. KHỞI TẠO TIẾN ĐỘ HỌC TẬP (UserLessonProgress)
                // Phải tạo record cho tất cả bài học trong khóa để track được bài nào học rồi/chưa
                await InitializeLessonProgressAsync(userId, courseId);

                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(newEnrollment.EnrollmentId);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        // Hàm phụ: Tạo progress rỗng cho học viên
        private async Task InitializeLessonProgressAsync(Guid userId, Guid courseId)
        {
            // Lấy tất cả Module -> Lấy tất cả Lesson
            var modules = await _unitOfWork.Modules.GetAllAsync(m => m.CourseId == courseId);
            foreach (var module in modules)
            {
                var lessons = await _unitOfWork.Lessons.GetAllAsync(l => l.ModuleId == module.ModuleId && !l.IsDeleted);

                foreach (var lesson in lessons)
                {
                    var progress = new UserLessonProgress
                    {
                        LessonProgressId = Guid.NewGuid(),
                        UserId = userId,
                        LessonId = lesson.LessonId,
                        IsCompleted = false,
                        CompletionPercent = 0,
                        LastWatchedSecond = 0
                    };
                    await _unitOfWork.UserLessonProgress.AddAsync(progress);
                }
            }
        }

        public async Task<ApiResponse> GetStudentEnrollmentsAsync()
        {
            var response = new ApiResponse();
            try
            {
                var userId = _claimService.GetUserClaim().UserId;

                // Lấy danh sách Enrollment + Include Course để hiển thị thông tin
                var enrollments = await _unitOfWork.Enrollments.GetQueryable()
                    .Include(e => e.Course)
                    .ThenInclude(c => c.User) // Lấy thêm tên giảng viên nếu cần
                    .Where(e => e.UserId == userId && e.Status == EnrollmentStatus.Active)
                    .OrderByDescending(e => e.EnrolledAt)
                    .ToListAsync();

                // Map sang ViewModel hoặc trả về List Enrollment tùy bạn (ở đây trả về list gốc cho nhanh)
                return response.SetOk(enrollments);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}