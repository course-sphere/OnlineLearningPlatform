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

                // 1. Kiểm tra Lesson tồn tại
                var lesson = await _unitOfWork.Lessons.GetAsync(l => l.LessonId == request.LessonId);
                if (lesson == null)
                {
                    return response.SetNotFound("Lesson not found or may have been automatically deleted due to inactivity!!! Please check your course");
                }

                if (request.File == null)
                {
                    return response.SetBadRequest(message: "Thêm File vào =,=");
                }

                // ===> LOGIC TỰ ĐỘNG TÍNH ORDER INDEX <===
                // 2. Lấy danh sách tài liệu hiện có của bài học này
                var existingResources = await _unitOfWork.LessonResources.GetAllAsync(r => r.LessonId == request.LessonId && !r.IsDeleted);

                // 3. Tìm số thứ tự lớn nhất + 1
                int newOrderIndex = existingResources.Any() ? existingResources.Max(r => r.OrderIndex) + 1 : 1;

                var lessonResource = _mapper.Map<LessonResource>(request);

                // 4. Upload lên Firebase
                var uploadResource = await _storage.UploadLessonResourceAsync(request.LessonId, request.Title, request.File);

                // 5. Gán các thông tin
                lessonResource.ResourceUrl = uploadResource.Url;
                lessonResource.ResourceType = uploadResource.Type;
                lessonResource.CreatedBy = claim.UserId;
                lessonResource.OrderIndex = newOrderIndex; // <== Gán Index tự động tại đây

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

                // Lấy resource chưa bị xóa
                var resources = await _unitOfWork.LessonResources.GetAllAsync(
                    r => r.LessonId == lessonId && !r.IsDeleted
                );

                // ===> SẮP XẾP TRƯỚC KHI TRẢ VỀ <===
                // Frontend sẽ hiển thị đúng thứ tự nhờ dòng này
                var sortedResources = resources.OrderBy(r => r.OrderIndex).ToList();

                var result = _mapper.Map<List<LessonResourceResponse>>(sortedResources);
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
                    // Logic xóa file cũ nếu cần thiết có thể bỏ comment
                    /* if (!string.IsNullOrEmpty(resource.ResourceUrl))
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