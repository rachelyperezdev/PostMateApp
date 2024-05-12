using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMateApp.Controllers;
using PostMateApp.Core.Application.Helpers;

namespace PostMateApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;
        private readonly IHttpContextAccessor _contextAccessor;
        public LoginAuthorize(ValidateUserSession userSession, IHttpContextAccessor contextAccessor)
        {
            _userSession = userSession;
            _contextAccessor = contextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var controller = (AccountController)context.Controller;
                context.Result = controller.RedirectToAction("Index", "Home");
            }
            else
            {
                await next();
            }
        }
    }
}
