

using DoAn.Helpers.ApiResponse;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace DoAn.Repositories.Email
{
    public class EmailRepositories : IEmailRepositories
    {
        private readonly IConfiguration _configuration;

        public EmailRepositories(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ApiResult<string>> SendEmail(string emailPeople, string subject , string content)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:Email"]));
                email.To.Add(MailboxAddress.Parse(emailPeople));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = GetHtmlcontent(subject,content) };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["EmailSettings:Host"], Int32.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["EmailSettings:Email"],_configuration["EmailSettings:Password"]);
                smtp.Send(email);
                smtp.Disconnect(true);
                return new ApiSuccessResult<string>("Gui thanh cong");
            }catch(Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
           
           
        }
        private string GetHtmlcontent(string subtitle, string content)
        {
            string Response = "<div style=\"width:100%;padding:10px 0;background-color:#FEBAD7;text-align:center;border-radius: 10px;\">";
            Response += $"<h1>{subtitle}</h1>";
            Response += "<img style=\"width : 100px\" src=\"https://pbs.twimg.com/profile_images/689189555765784576/3wgIDj3j_400x400.png\" />";
            Response += $"<h2 style=\"font-size: 16px;width: 600px;margin: 20px auto;\">{content}</h2>";
            Response += "<div><h4> Liên hệ với tôi : bthanh2001@gmail.com</h4></div>";
            Response += "</div>";
            return Response;
        }
    }
}
