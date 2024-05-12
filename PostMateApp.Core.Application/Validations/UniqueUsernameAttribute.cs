using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PostMateApp.Core.Application.Interfaces.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace PostMateApp.Core.Application.Validations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));

            var username = (string)value;
            if(string.IsNullOrEmpty(username))
            {
                return new ValidationResult(ErrorMessage ?? "Debe ingresar un nombre de usuario.");
            }

            var currentUserId = GetCurrentUserId(validationContext);

            var existingUser = userService.GetUserByUsernameAsync(username).GetAwaiter().GetResult();
            if (existingUser != null && existingUser.Id != currentUserId)
            {
                return new ValidationResult(ErrorMessage ?? "Ese nombre de usuario ya está en uso. Por favor, elija otro.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        private string GetCurrentUserId(ValidationContext validationContext)
        {
            HttpContext httpContext = validationContext.GetService<IHttpContextAccessor>().HttpContext;

            string userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
