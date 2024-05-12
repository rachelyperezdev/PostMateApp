using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMateApp.Controllers;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.Helpers;

namespace PostMateApp.Middlewares
{
    public class ProfileOwnerAuthorize : IAsyncActionFilter
    {
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProfileOwnerAuthorize(IPostService postService, IHttpContextAccessor contextAccessor)
        {
            _postService = postService;
            _contextAccessor = contextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            object authentication = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            var attemptedHomeAccess = context.HttpContext.Request.Path.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase);
            var attemptedEditAccess = context.HttpContext.Request.Path.StartsWithSegments("/Account/Edit");
            var attemptedFriendAccess = context.HttpContext.Request.Path.StartsWithSegments("/Friend/Index");

            if ((attemptedHomeAccess || attemptedEditAccess || attemptedFriendAccess) && authentication == null)
            {
                context.HttpContext.Response.Cookies.Append("ErrorMessage", "Debe iniciar sesión para acceder a esta página.");
                context.Result = new RedirectToActionResult("Index", "Account", null);
                return;
            }


            if (authentication == null)
            {
                context.Result = new RedirectToActionResult("Index", "Account", null);
                return;
            }

            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Account", null);
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Account", null);
                return;
            }

            if (context.Controller.GetType() == typeof(PostController))
            {
                var postId = context.HttpContext.Request.RouteValues["id"]?.ToString();

                if (string.IsNullOrEmpty(postId))
                {
                    var controller = (AccountController)context.Controller;
                    context.Result = controller.RedirectToAction("Index", "Account");
                    return;
                }

                if (!await _postService.IsPostOwner(postId))
                {
                    var controller = (PostController)context.Controller;
                    context.Result = controller.RedirectToAction("Index", "Home");
                    return;
                }
            }

            if (context.Controller.GetType() == typeof(AccountController))
            {
                var currentUserPath = $"/Account/Edit/{_contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id}";
                var requestedUserUserPath = context.HttpContext.Request.Path.ToString();

                if (currentUserPath != requestedUserUserPath)
                {
                    var controller = (AccountController)context.Controller;
                    context.Result = controller.RedirectToAction("Index", "Home");
                    return;
                }
            }

            await next();
        }
    }
}
