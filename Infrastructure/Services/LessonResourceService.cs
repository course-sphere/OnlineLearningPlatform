using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.LessonResource;
using Domain.Responses;
using Domain.Responses.LessonResource;
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
                var claim = _service.GetUserClaim();
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
                lessonResource.CreatedBy = claim.UserId;    
                await _unitOfWork.LessonResources.AddAsync(lessonResource);
                await _unitOfWork.SaveChangeAsync();
                var result = _mapper.Map<LessonResourceResponse>(lessonResource);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
        public async Task<ApiResponse> GetResourcesByLessonAsync(Guid lessonId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == lessonId);
                if (lesson == null)
                    return response.SetNotFound("Lesson not found");

                var resources = await _unitOfWork.LessonResources.GetAllAsync(
                    r => r.LessonId == lessonId
                );

                var result = _mapper.Map<List<LessonResourceResponse>>(resources);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateLessonResourceAsync(Guid resourceId, UpdateLessonResourceRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var resource = await _unitOfWork.LessonResources
                    .GetAsync(r => r.LessonResourceId == resourceId);

                if (resource == null)
                    return response.SetNotFound("Lesson resource not found");

                // Update basic info
                resource.Title = request.Title ?? resource.Title;
                resource.UpdatedBy = _service.GetUserClaim().UserId;

                // Update file nếu có
                if (request.File != null)
                {
                    // Xóa file cũ
                 /*   if (!string.IsNullOrEmpty(resource.ResourceUrl))
                    {
                        await _storage.DeleteAsync(resource.ResourceUrl);
                    }*/

                    var upload = await _storage.UploadLessonResourceAsync(
                        resource.LessonId,
                        resource.Title,
                        request.File
                    );

                    resource.ResourceUrl = upload.Url;
                    resource.ResourceType = upload.Type;
                }

                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Lesson resource updated successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteLessonResourceAsync(Guid resourceId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var resource = await _unitOfWork.LessonResources
                    .GetAsync(r => r.LessonResourceId == resourceId);

                if (resource == null)
                    return response.SetNotFound("Lesson resource not found");

                // Delete file trên Firebase
       /*         if (!string.IsNullOrEmpty(resource.ResourceUrl))
                {
                    await _storage.DeleteAsync(resource.ResourceUrl);
                }
*/
                _unitOfWork.LessonResources.RemoveIdAsync(resource.LessonResourceId);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Lesson resource deleted successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

    }
}
