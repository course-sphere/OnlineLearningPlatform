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

        public GradedItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
    }
}
