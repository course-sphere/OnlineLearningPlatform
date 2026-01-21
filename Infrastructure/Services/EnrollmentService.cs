using Application;
using Application.IServices;
using AutoMapper;
using Domain.Requests.Enrollment;
using Domain.Responses;

namespace Infrastructure.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IClaimService _service;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, IClaimService service)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _service = service;
        }

        public async Task<ApiResponse> CreateNewEnrollmentAsync(CreateNewEnrollementRequest request)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();
                var enrollment = await _unitOfWork.Enrollments.GetAsync(e => e.CourseId == request.CourseId && e.UserId == claim.UserId);
                if (enrollment != null)
                {
                    return response.SetBadRequest(message: "Enrollment had created with Course");
                }

                return null;//var payment = await _paymentService
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        } 
    }
}
