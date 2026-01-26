using Application;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
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
                var existingEnrollment = await _unitOfWork.Enrollments.GetAsync(e => e.CourseId == request.CourseId && e.UserId == claim.UserId);
                if (existingEnrollment != null)
                {
                    return response.SetBadRequest(message: "Enrollment had created with Course");
                }

                var payment = await _unitOfWork.Payments.GetAsync(p => p.CourseId == request.CourseId && p.UserId == claim.UserId && p.IsSuccess == true);
                if (payment == null)
                {
                    return response.SetBadRequest(message: "Payment not found for this course or not success");
                }

                var enrollment = _mapper.Map<Enrollment>(request);
                await _unitOfWork.Enrollments.AddAsync(enrollment);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk("Create");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
