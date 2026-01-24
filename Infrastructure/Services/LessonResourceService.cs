using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.LessonResource;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class LessonResourceService : ILessonResourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _service;
        private readonly IFirebaseStorageService _storage;

        public LessonResourceService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService service, IFirebaseStorageService storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _service = service;
            _storage = storage;
        }

        public async Task<ApiResponse> CreateLessonResourceAsync(CreateLessonResourceRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == request.LessonId);
                if (lesson == null)
                {
                    return response.SetNotFound("Lesson not found or may have been automatically deleted due to inactivity!!! Please check your course");
                }

                var lessonResource = _mapper.Map<LessonResource>(request);
                if (request.File == null)
                {
                    return response.SetBadRequest(message: "Thêm File vào =,=");
                }
                var uploadResource = await _storage.UploadLessonResourceAsync(request.LessonId, request.Title, request.File);
                lessonResource.ResourceUrl = uploadResource.Url;
                lessonResource.ResourceType = uploadResource.Type;
                await _unitOfWork.LessonResources.AddAsync(lessonResource);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk(lessonResource);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
