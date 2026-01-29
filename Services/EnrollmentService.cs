using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Enrollment;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IClaimService _service;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewEnrollmentAsync(CreateNewEnrollementRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();
                var existingEnrollment = await _unitOfWork.Enrollments.GetAsync(e => e.CourseId == request.CourseId && e.UserId == claim.UserId);
                if (existingEnrollment != null)
                {
                    return response.SetBadRequest(message: "Enrollment had created with Course");
                }

                var payment = await _unitOfWork.Payments.GetAsync(p => p.CourseId == request.CourseId && p.UserId == claim.UserId && p.IsSuccess == true);
                if (payment == null)
                {
                    return response.SetBadRequest(message: "Payment not found for this course or not success");
                }

                var enrollment = _mapper.Map<Enrollment>(request);
                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Create");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> EnrollStudentDirectlyAsync(Guid courseId)
        {
            var response = new ApiResponse();
            try
            {
                var userId = _service.GetUserClaim().UserId;

                // 1. Check xem đã enroll chưa (nếu có rồi thì trả về ID luôn để redirect)
                var existing = await _unitOfWork.Enrollments.GetAsync(e => e.CourseId == courseId && e.UserId == userId);
                if (existing != null)
                {
                    return response.SetOk(existing.EnrollmentId);
                }

                // 2. TẠO ENROLLMENT (Set luôn là Active)
                var newEnrollment = new Enrollment
                {
                    EnrollmentId = Guid.NewGuid(),
                    CourseId = courseId,
                    UserId = userId,
                    EnrolledAt = DateTime.UtcNow,
                    Status = EnrollmentStatus.Active, // <--- TRICK Ở ĐÂY: Active luôn
                    ProgressPercent = 0
                };

                await _unitOfWork.Enrollments.AddAsync(newEnrollment);

                // 3. TẠO DATA TIẾN ĐỘ HỌC (Quan trọng cho Flow Learning)
                await InitializeLessonProgressAsync(userId, courseId);

                // 4. Lưu DB
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(newEnrollment.EnrollmentId);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetStudentEnrollmentsAsync()
        {
            var response = new ApiResponse();
            try
            {
                var userId = _service.GetUserClaim().UserId;

                // Lấy enrollment + Course info + Giảng viên
                var enrollments = await _unitOfWork.Enrollments.GetQueryable()
                    .Include(e => e.Course)
                    .Where(e => e.UserId == userId && e.Status == EnrollmentStatus.Active)
                    .OrderByDescending(e => e.EnrolledAt)
                    .ToListAsync();

                return response.SetOk(enrollments);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        private async Task InitializeLessonProgressAsync(Guid userId, Guid courseId)
        {
            // Lấy tất cả Module -> Lấy tất cả Lesson -> Tạo record Progress = 0%
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
                    // Dùng Generic Repository đã khai báo trong UnitOfWork
                    await _unitOfWork.UserLessonProgress.AddAsync(progress);
                }
            }
        }


    }
}
