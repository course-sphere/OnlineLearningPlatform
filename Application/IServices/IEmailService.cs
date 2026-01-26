using Domain.Responses;

namespace Application.IServices
{
    public interface IEmailService
    {
        Task<ApiResponse> SendRejectCourseEmail(
    string receiverName,
    string receiverEmail,
    string rejectReason,
    string courseTitle);
    }
}
