using Application.IServices;
using Domain;
using Domain.Responses;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        public EmailService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public async Task<ApiResponse> SendRejectCourseEmail(
    string receiverName,
    string receiverEmail,
    string rejectReason,
    string courseTitle)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var htmlTemplate = @"
            <h3>Course Rejected</h3>
            <p>Hello <b>{{Name}}</b>,</p>
            <p>Your course <b>{{CourseTitle}}</b> has been rejected.</p>
            <p><b>Reason:</b> {{RejectReason}}</p>
            <p>Please review and resubmit.</p>
            <br/>
            <p>Best regards,<br/>HuyShop Team</p>";

                htmlTemplate = htmlTemplate
                    .Replace("{{Name}}", receiverName)
                    .Replace("{{CourseTitle}}", courseTitle)
                    .Replace("{{RejectReason}}", rejectReason);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("HuyShop", _appSettings.SMTP.Email));
                message.To.Add(new MailboxAddress(receiverName, receiverEmail));
                message.Subject = "Your course has been rejected";

                message.Body = new BodyBuilder
                {
                    HtmlBody = htmlTemplate
                }.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(_appSettings.SMTP.Email, _appSettings.SMTP.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return response.SetOk("Reject email sent");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
