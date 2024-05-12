using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Helpers;

namespace PostMateApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if(userViewModel == null)
            {
                return false;
            }
            return true;
        }
    }
}
