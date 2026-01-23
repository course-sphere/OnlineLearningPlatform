using Application;
using Application.IServices;
using Domain.Entities;
using Domain.Requests.Payment;
using Domain.Responses;
using Infrastructure.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
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
                var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_config["TimeZoneId"]);
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                var tick = DateTime.Now.Ticks.ToString();
                var pay = new VnPayLibrary();

                var claim = _service.GetUserClaim();
                var course = await _unitOfWork.Courses.GetAsync(c => c.CourseId == request.CourseId);
                if (course == null)
                {
                    return response.SetNotFound(message: "Course not found");
                }

                var payment = new Payment();
                payment.Amount = course.Price;
                payment.CourseId = request.CourseId;
                payment.Method = "VnPay";

                var urlCallBack = $"{_config["PaymentCallBack:ReturnUrl"]}?userId={claim.UserId}&amount={payment.Amount}";

                pay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
                pay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
                pay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
                pay.AddRequestData("vnp_Amount", ((int)payment.Amount * 100).ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
                pay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]);
                pay.AddRequestData("vnp_OrderInfo", $"{course.Price}");
                pay.AddRequestData("vnp_OrderType", "VnPay");
                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                pay.AddRequestData("vnp_TxnRef", tick);

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

                if (!vnPayResponse.VnPayResponseCode.Equals("00")) return response.SetBadRequest(message: "Payment Cancel!!!");

                if (collection.TryGetValue("userId", out var userIdValue) && Guid.TryParse(userIdValue, out Guid userId))
                {
                    if (vnPayResponse.Success)
                    {
                        var user = await _unitOfWork.Users.GetAsync(u => u.UserId == userId);

                        if (user != null)
                        {
                            if (collection.TryGetValue("amount", out var amountValue) && int.TryParse(amountValue, out int amount))
                            {
                                var course = await _unitOfWork.Courses.GetAsync(c => c.Price == amount);
                                if (course == null) return response.SetBadRequest("Invalid payment amount");
                            }
                            else
                            {
                                return response.SetBadRequest("Parse error");
                            }

                            await _unitOfWork.SaveChangeAsync();
                            return response.SetOk();
                        }
                        else
                        {
                            return response.SetNotFound("User Not Found");
                        }
                    }
                    else
                    {
                        return response.SetBadRequest(message
                            : "VNPay API Response Fail");
                    }
                }
                else
                {
                    return response.SetBadRequest("Invalid or missing userId from callback url");
                }
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(message: ex.Message);
            }
        }
    }
}
