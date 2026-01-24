using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.UserLessonProgress;
using Domain.Responses;

namespace Infrastructure.Services
{
    public class UserLessonProgressService : IUserLessonProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _service;

        public UserLessonProgressService(IUnitOfWork unitOfWork, IClaimService service
            )
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public Task<ApiResponse> CompleteLessonAsync(Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetProgressByLessonAsync(Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> StartLessonAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userId = _service.GetUserClaim().UserId;

                var lesson = await _unitOfWork.Lessons
                    .GetAsync(l => l.LessonId == lessonId);

                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                var progress = await _unitOfWork.LessonProgresses
                    .GetAsync(p => p.UserId == userId && p.LessonId == lessonId);

                if (progress != null)
                    return response.SetOk("Lesson already started");

                progress = new UserLessonProgress
                {
                    LessonProgressId = Guid.NewGuid(),
                    UserId = userId,
                    LessonId = lessonId,
                    IsCompleted = false,
                    LastWatchedSecond = 0,
                    LastAccessedAt = DateTime.UtcNow
                };

                await _unitOfWork.LessonProgresses.AddAsync(progress);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Lesson started");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public Task<ApiResponse> UpdateProgressAsync(UpdateUserLessonProgressRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
