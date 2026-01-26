using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Module;
using Domain.Responses;
using Domain.Responses.Module;

namespace Infrastructure.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _service;

        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewModuleForCourseAsync(CreateNewModuleForCourseRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == request.CourseId);
                if (course == null)
                {
                    return response.SetNotFound(message: "Course not found or may have been automatically deleted due to inactivity!!!");
                }

                var module = _mapper.Map<Module>(request);
                module.CreatedBy = _service.GetUserClaim().UserId;
                await _unitOfWork.Modules.AddAsync(module);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk($"Created Module {module.Index} {module.Name} successfully ^^");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteModuleAsync(Guid moduleId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var module = await _unitOfWork.Modules
                    .GetAsync(m => m.ModuleId == moduleId && !m.IsDeleted);

                if (module == null)
                    return response.SetNotFound("Module not found");

                module.IsDeleted = true;
                module.UpdatedAt = DateTime.UtcNow;
                module.UpdatedBy = _service.GetUserClaim().UserId;

                _unitOfWork.Modules.Update(module);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Module deleted successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> GetModuleDetailAsync(Guid moduleId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var module = await _unitOfWork.Modules
                    .GetAsync(m => m.ModuleId == moduleId && !m.IsDeleted);

                if (module == null)
                    return response.SetNotFound("Module not found");

                var result = _mapper.Map<ModuleResponse>(module);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> GetModulesByCourseAsync(Guid courseId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var modules = await _unitOfWork.Modules
                    .GetAllAsync(m => m.CourseId == courseId && !m.IsDeleted);

                var result = _mapper.Map<List<ModuleResponse>>(modules);
                return response.SetOk(result);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateModuleAsync(UpdateModuleRequest request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var module = await _unitOfWork.Modules
                    .GetAsync(m => m.ModuleId == request.ModuleId && !m.IsDeleted);

                if (module == null)
                    return response.SetNotFound("Module not found");

                _mapper.Map(request, module);
                module.UpdatedAt = DateTime.UtcNow;
                module.UpdatedBy = _service.GetUserClaim().UserId;

                _unitOfWork.Modules.Update(module);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Module updated successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
