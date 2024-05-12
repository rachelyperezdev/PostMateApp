using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Domain.Settings;
using PostMateApp.Infrastructure.Shared.Services;

namespace PostMateApp.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
