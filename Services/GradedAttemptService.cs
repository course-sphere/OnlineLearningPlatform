using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Services
{
    public class GradedAttemptService : IGradedAttemptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _claimService;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _storage;

        public GradedAttemptService(IUnitOfWork unitOfWork, IClaimService claimService, IMapper mapper, IFirebaseStorageService storage)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimService;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<ApiResponse> StartAttemptAsync(Guid gradedItemId)
        {
            ApiResponse response = new ApiResponse();
            var userId = _claimService.GetUserClaim().UserId;

            var gradedItem = await _unitOfWork.GradedItems
                .GetAsync(x => x.GradedItemId == gradedItemId);

            if (gradedItem == null)
                return response.SetNotFound("Graded item not found");

            var attemptCount = await _unitOfWork.GradedAttempts.CountAsync(
                x => x.UserId == userId && x.GradedItemId == gradedItemId);

            var attempt = new GradedAttempt
            {
                GradedAttemptId = Guid.NewGuid(),
                UserId = userId,
                GradedItemId = gradedItemId,
                AttemptNumber = attemptCount + 1,
                Status = GradedAttemptStatus.InProgress,
                SubmittedAt = DateTime.UtcNow
            };

            await _unitOfWork.GradedAttempts.AddAsync(attempt);
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk(attempt.GradedAttemptId);
        }
        public async Task<ApiResponse> SubmitShortAnswerAsync(Guid attemptId, Guid questionId, string answer, IFormFile? file)
        {
            ApiResponse response = new ApiResponse();

            var attempt = await _unitOfWork.GradedAttempts
                .GetAsync(x => x.GradedAttemptId == attemptId);

            if (attempt == null)
                return response.SetNotFound("Attempt not found");

            if (attempt.Status != GradedAttemptStatus.InProgress)
                return response.SetBadRequest("Attempt already submitted");

            var submission = new QuestionSubmission
            {
                QuestionSubmissionId = Guid.NewGuid(),
                GradedAttemptId = attemptId,
                QuestionId = questionId,
                AnswerText = answer,
                FileUrl = file != null ? await _storage.UploadQuestionSubmissionFile(file) : null,
            };

            await _unitOfWork.QuestionSubmissions.AddAsync(submission);
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk("Answer submitted");
        }

        public async Task<ApiResponse> SubmitAttemptAsync(Guid attemptId)
        {
            ApiResponse response = new ApiResponse();

            var attempt = await _unitOfWork.GradedAttempts.GetAsync(
                x => x.GradedAttemptId == attemptId,
                include: q => q
                    .Include(a => a.GradedItem)
                        .ThenInclude(g => g.Lesson)
                            .ThenInclude(l => l.Module)
                    .Include(a => a.QuestionSubmissions)
                        .ThenInclude(q => q.Question)
                            .ThenInclude(q => q.AnswerOptions));

            if (attempt == null)
                return response.SetNotFound("Attempt not found");

            attempt.Status = GradedAttemptStatus.Submitted;
            attempt.SubmittedAt = DateTime.UtcNow;
            bool hasManualQuestion = false;

            //Auto Graded
            if (attempt.GradedItem!.IsAutoGraded)
            {
                decimal totalScore = 0;

                foreach (var submission in attempt.QuestionSubmissions!)
                {
                    var question = submission.Question!;

                    // ❌ ShortAnswer → để instructor chấm
                    if (question.Type == QuestionType.ShortAnswer)
                    {
                        hasManualQuestion = true;
                        continue;
                    }

                    // ✅ MultipleChoice / TrueFalse
                    var correctAnswerIds = question.AnswerOptions!
                        .Where(x => x.IsCorrect)
                        .Select(a => a.AnswerOptionId)
                        .ToHashSet();
                    //Dap an student chon
                    var selectedAnswerIds = submission.SelectedOptions!
                        .Select(s => s.AnswerOptionId)
                        .ToHashSet();
                    bool isCorrect = selectedAnswerIds.SetEquals(correctAnswerIds);
                    if (isCorrect)
                    {
                        submission.Score = question.Points;
                        totalScore += question.Points;
                    }
                    else
                    {
                        submission.Score = 0;
                    }
                }

                // 🎯 Chỉ auto-grade hoàn toàn khi KHÔNG có câu tự luận
                if (!hasManualQuestion)
                {
                    attempt.Score = totalScore;
                    attempt.Status = GradedAttemptStatus.Graded;
                    attempt.GradedAt = DateTime.UtcNow;

                    await UpdateLessonAndEnrollmentProgress(attempt);
                }
            }

            _unitOfWork.GradedAttempts.Update(attempt);
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk(
                hasManualQuestion
                    ? "Attempt submitted, waiting for instructor grading"
                    : "Attempt auto graded successfully");
        }

        public async Task<ApiResponse> GradeAssignmentAsync(Guid attemptId, decimal score)
        {
            ApiResponse response = new ApiResponse();

            var attempt = await _unitOfWork.GradedAttempts.GetAsync(
                x => x.GradedAttemptId == attemptId,
                include: q => q
                    .Include(a => a.GradedItem)
                        .ThenInclude(g => g.Lesson)
                            .ThenInclude(l => l.Module));

            if (attempt == null)
                return response.SetNotFound("Attempt not found");

            attempt.Score = score;
            attempt.Status = GradedAttemptStatus.Graded;
            attempt.GradedAt = DateTime.UtcNow;

            await UpdateLessonAndEnrollmentProgress(attempt);

            _unitOfWork.GradedAttempts.Update(attempt);
            await _unitOfWork.SaveChangeAsync();

            return response.SetOk("Assignment graded");
        }

        private async Task UpdateLessonAndEnrollmentProgress(GradedAttempt attempt)
        {
            var userId = attempt.UserId;
            var lessonId = attempt.GradedItem!.Lesson.LessonId;
            var courseId = attempt.GradedItem.Lesson.Module!.CourseId;

            // LessonProgress
            var lessonProgress = await _unitOfWork.LessonProgresses.GetAsync(
                x => x.UserId == userId && x.LessonId == lessonId);

            if (lessonProgress != null)
            {
                lessonProgress.IsCompleted = true;
                lessonProgress.CompletionPercent = 100;
                lessonProgress.CompletedAt = DateTime.UtcNow;

                _unitOfWork.LessonProgresses.Update(lessonProgress);
            }

            // Enrollment Progress
            var totalLessons = await _unitOfWork.Lessons.CountAsync(
                x => x.Module!.CourseId == courseId);

            var completedLessons = await _unitOfWork.LessonProgresses.CountAsync(
                x => x.UserId == userId && x.IsCompleted);

            var enrollment = await _unitOfWork.Enrollments.GetAsync(
                x => x.UserId == userId && x.CourseId == courseId);

            if (enrollment != null)
            {
                enrollment.ProgressPercent =
                    Math.Round(completedLessons * 100m / totalLessons, 2);

                if (enrollment.ProgressPercent == 100)
                {
                    enrollment.Status = EnrollmentStatus.Completed;
                    enrollment.CompletedAt = DateTime.UtcNow;
                }

                _unitOfWork.Enrollments.Update(enrollment);
            }
        }
    }
}
