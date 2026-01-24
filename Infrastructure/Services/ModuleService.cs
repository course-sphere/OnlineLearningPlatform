using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Requests.Module;
using Domain.Responses;

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
    }
}
