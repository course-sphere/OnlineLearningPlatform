using Application;
using Application.IServices;
using Domain.Entities;
using Domain.Requests.Payment;
using Domain.Responses;
using Services.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimService _service;

        public PaymentService(IConfiguration config, IUnitOfWork unitOfWork, IClaimService service)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<ApiResponse> CreatePaymentUrlAsync(CreateNewPaymentRequest request, HttpContext context)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var claim = _service.GetUserClaim();
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == request.CourseId);
                if (course == null)
                {
                    return response.SetNotFound(message: "Course not found");
                }

                var enrollment = await _unitOfWork.Enrollments.GetAsync(e => e.UserId == claim.UserId && e.CourseId == course.CourseId);

                if (enrollment == null)
                {
                    enrollment = new Enrollment
                    {
                        EnrollmentId = Guid.NewGuid(),
                        UserId = claim.UserId,
                        CourseId = course.CourseId,
                        Status = EnrollmentStatus.PendingPayment
                    };

                    await _unitOfWork.Enrollments.AddAsync(enrollment);
                }
                else
                {
                    enrollment.Status = EnrollmentStatus.PendingPayment;
                    enrollment.UpdatedAt = DateTime.UtcNow;
                }
                await _unitOfWork.SaveChangeAsync();

                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_config["TimeZoneId"]);
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                var tick = DateTime.Now.Ticks.ToString();
                var pay = new VnPayLibrary();

                var txnRef = $"{claim.UserId}|{course.CourseId}|{DateTime.UtcNow.Ticks}";

                var urlCallBack = $"{_config["PaymentCallBack:ReturnUrl"]}?userId={claim.UserId}&amount={course.Price}";

                pay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
                pay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
                pay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
                pay.AddRequestData("vnp_Amount", ((int)course.Price * 100).ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
                pay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]);
                pay.AddRequestData("vnp_OrderInfo", $"{course.Price}");
                pay.AddRequestData("vnp_OrderType", "VnPay");
                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                pay.AddRequestData("vnp_TxnRef", txnRef);

                var paymentUrl = pay.CreateRequestUrl(_config["Vnpay:BaseUrl"], _config["Vnpay:HashSecret"]);

                return response.SetOk(paymentUrl);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }

        public async Task<ApiResponse> PaymentExecuteAsync(IQueryCollection collection)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var pay = new VnPayLibrary();
                var vnPayResponse = pay.GetFullResponseData(collection, _config["Vnpay:HashSecret"]);

                if (!collection.TryGetValue("vnp_TxnRef", out var txnRefValue))
                    return response.SetBadRequest("Missing vnp_TxnRef");

                var parts = txnRefValue.ToString().Split('|');
                if (parts.Length < 2)
                    return response.SetBadRequest("Invalid TxnRef format");

                if (!Guid.TryParse(parts[0], out Guid userId))
                    return response.SetBadRequest("Invalid userId");

                if (!Guid.TryParse(parts[1], out Guid courseId))
                    return response.SetBadRequest("Invalid courseId");

                var enrollment = await _unitOfWork.Enrollments.GetAsync(e =>
                    e.UserId == userId &&
                    e.CourseId == courseId
                );

                if (enrollment == null)
                    return response.SetNotFound("Enrollment not found");

                if (enrollment.Status != EnrollmentStatus.PendingPayment)
                {
                    return response.SetOk("Payment already processed");
                }
                   
                if (vnPayResponse.VnPayResponseCode != "00")
                {
                    enrollment.Status = EnrollmentStatus.Cancelled;

                    await _unitOfWork.Payments.AddAsync(new Payment
                    {
                        PaymentId = Guid.NewGuid(),
                        UserId = userId,
                        CourseId = courseId,
                        Amount = enrollment.Course!.Price,
                        Method = "VnPay",
                    });

                    await _unitOfWork.SaveChangeAsync();
                    return response.SetBadRequest("Payment failed");
                }

                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == enrollment.CourseId);
                if (course == null) return response.SetNotFound("Course not found");
                // 2️⃣ Tạo Payment
                var payment = new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    UserId = userId,
                    CourseId = courseId,
                    Amount = course.Price,
                    Method = "VnPay",
                    IsSuccess = true,
                };

                enrollment.Status = EnrollmentStatus.Active;

                await _unitOfWork.Payments.AddAsync(payment);
                await _unitOfWork.SaveChangeAsync();

                return response.SetOk("Payment created successfully ^^");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
