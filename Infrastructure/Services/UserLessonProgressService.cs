using Application;
using Application.IServices;
using Domain.Entities;
using Domain.Requests.UserLessonProgress;
using Domain.Responses;

namespace Infrastructure.Services
{
    public class UserLessonProgressService : IUserLessonProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _claimService;

        public UserLessonProgressService(
            IUnitOfWork unitOfWork,
            IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimService;
        }

        public async Task<ApiResponse> StartOrUpdateProgressAsync(UpdateUserLessonProgressRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userId = _claimService.GetUserClaim().UserId;

                var lesson = await _unitOfWork.Lessons
                    .GetAsync(l => l.LessonId == request.LessonId);

                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                var progress = await _unitOfWork.LessonProgresses.GetAsync(
                    p => p.UserId == userId && p.LessonId == request.LessonId);

                // Chưa có → tạo mới
                if (progress == null)
                {
                    progress = new UserLessonProgress
                    {
                        LessonProgressId = Guid.NewGuid(),
                        UserId = userId,
                        LessonId = request.LessonId,
                        LastWatchedSecond = request.LastWatchedSecond,
                        CompletionPercent = request.CompletionPercent,
                        LastAccessedAt = DateTime.UtcNow,
                        IsCompleted = false
                    };

                    await _unitOfWork.LessonProgresses.AddAsync(progress);
                }
                else
                {
                    progress.LastWatchedSecond = request.LastWatchedSecond ?? progress.LastWatchedSecond;
                    progress.CompletionPercent = request.CompletionPercent ?? progress.CompletionPercent;
                    progress.LastAccessedAt = DateTime.UtcNow;

                    _unitOfWork.LessonProgresses.Update(progress);
                }

                await _unitOfWork.SaveChangeAsync();
                return response.SetOk(progress);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> MarkLessonCompletedAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userId = _claimService.GetUserClaim().UserId;

                var progress = await _unitOfWork.LessonProgresses.GetAsync(
                    p => p.UserId == userId && p.LessonId == lessonId);

                if (progress == null)
                    return response.SetNotFound("Lesson progress not found");

                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
                progress.CompletionPercent = 100;

                _unitOfWork.LessonProgresses.Update(progress);
                var courseId = progress.Lesson.Module.CourseId;

                var totalLessonInCourse = await _unitOfWork.Lessons
                    .CountAsync(l => l.Module.CourseId == courseId);

                var completedLessonsInCourse = await _unitOfWork.LessonProgresses.CountAsync(
                    lp => lp.UserId == userId
                       && lp.IsCompleted
                       && lp.Lesson.Module.CourseId == courseId
                );

                var enrollment = await _unitOfWork.Enrollments
                    .GetAsync(e => e.UserId == userId && e.CourseId == courseId);   
                if (enrollment != null && totalLessonInCourse > 0)
                {
                    enrollment.ProgressPercent = Math.Round(completedLessonsInCourse * 100m / totalLessonInCourse, 2);
                    if (enrollment.ProgressPercent >= 100)
                    {
                        enrollment.ProgressPercent = 100;
                        enrollment.Status = EnrollmentStatus.Completed;
                        enrollment.CompletedAt = DateTime.UtcNow;
                    }
                    else if (completedLessonsInCourse > 0)
                    {
                        enrollment.Status = EnrollmentStatus.Active;
                    }
                    
                }
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Lesson marked as completed");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        
        public async Task<ApiResponse> GetLessonProgressAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userId = _claimService.GetUserClaim().UserId;

                var progress = await _unitOfWork.LessonProgresses.GetAsync(
                    p => p.UserId == userId && p.LessonId == lessonId);

                return response.SetOk(progress);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetLessonProgressByUserAsync(Guid userId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var progresses = await _unitOfWork.LessonProgresses
                    .GetAllAsync(p => p.UserId == userId);

                return response.SetOk(progresses);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
