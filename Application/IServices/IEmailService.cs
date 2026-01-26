using Domain.Responses;

namespace Application.IServices
{
    public interface IEmailService
    {
        Task<ApiResponse> SendRejectCourseEmail(string receiverName, string receiverEmail, string rejectReason, string courseTitle);

        Task<ApiResponse> SendApproveCourseEmail(string receiverName, string receiverEmail, string courseTitle);
    }
}
