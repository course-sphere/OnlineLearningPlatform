using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.GradedItem;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GradedItemService : IGradedItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _service;

        public GradedItemService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewGradedItemAsync(CreateGradedItemRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons
                    .GetAsync(x => x.LessonId == request.LessonId);

                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                var gradedItem = _mapper.Map<GradedItem>(request);

                gradedItem.GradedItemId = Guid.NewGuid();
                gradedItem.IsAutoGraded = request.Type == GradedItemType.Quiz;
                gradedItem.Lesson = lesson;

                if (request.Questions != null && request.Questions.Any())
                {
                    gradedItem.Questions = request.Questions.Select(q =>
                    {
                        var question = _mapper.Map<Question>(q);
                        question.QuestionId = Guid.NewGuid();
                        question.GradedItemId = gradedItem.GradedItemId;

                        if (q.AnswerOptions != null)
                        {
                            question.AnswerOptions = q.AnswerOptions.Select(a =>
                                new AnswerOption
                                {
                                    AnswerOptionId = Guid.NewGuid(),
                                    Text = a.Text,
                                    IsCorrect = a.IsCorrect
                                }).ToList();
                        }

                        return question;
                    }).ToList();
                }

                await _unitOfWork.GradedItems.AddAsync(gradedItem);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Graded item created successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
        public async Task<ApiResponse> SubmitQuizAsync(SubmitQuizRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userId = _service.GetUserClaim().UserId;

                var gradedItem = await _unitOfWork.GradedItems
                    .GetAsync(g => g.GradedItemId == request.GradedItemId);

                if (gradedItem == null)
                    return response.SetNotFound("Quiz not found");

                if (!gradedItem.IsAutoGraded || gradedItem.Type != GradedItemType.Quiz)
                    return response.SetBadRequest("This quiz is not auto-graded");

                var attempt = new GradedAttempt
                {
                    GradedAttemptId = Guid.NewGuid(),
                    GradedItemId = gradedItem.GradedItemId,
                    UserId = userId,
                    SubmittedAt = DateTime.UtcNow,
                    QuestionSubmissions = new()
                };

                decimal totalScore = 0;

                foreach (var answer in request.Answers)
                {
                    var question = gradedItem.Questions
                        .First(q => q.QuestionId == answer.QuestionId);

                    if (question.Type == QuestionType.ShortAnswer)
                        continue;

                    var correctOptions = await _unitOfWork.AnswerOptions
                        .GetAllAsync(a => a.QuestionId == question.QuestionId && a.IsCorrect);

                    var isCorrect =
                        correctOptions.Select(o => o.AnswerOptionId).OrderBy(x => x)
                        .SequenceEqual(answer.SelectedAnswerOptionIds.OrderBy(x => x));

                    var questionScore = isCorrect ? question.Points : 0;

                    totalScore += questionScore;

                    attempt.QuestionSubmissions.Add(new QuestionSubmission
                    {
                        QuestionSubmissionId = Guid.NewGuid(),
                        QuestionId = question.QuestionId,
                        IsCorrect = isCorrect,
                        Score = questionScore,
                        SubmissionAnswerOptions = answer.SelectedAnswerOptionIds
                            .Select(id => new SubmissionAnswerOption
                            {
                                AnswerOptionId = id
                            }).ToList()
                    });
                }

                attempt.Score = totalScore;

                await _unitOfWork.GradedAttempts.AddAsync(attempt);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk(new
                {
                    attempt.Score,
                    MaxScore = gradedItem.MaxScore,
                    attempt.IsSubmitted
                });
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

    }
}
