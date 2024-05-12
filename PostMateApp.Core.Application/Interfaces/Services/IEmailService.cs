using PostMateApp.Core.Application.DTOs.Email;
using PostMateApp.Core.Domain.Settings;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}
