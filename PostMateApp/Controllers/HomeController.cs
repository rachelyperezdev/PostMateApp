using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Middlewares;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Core.Application.DTOs.Account;

namespace PostMateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IFriendshipService _friendshipService; 

        public HomeController(IUserService userService, IPostService postService, ICommentService commentService, IReplyService replyService, IFriendshipService friendshipService)
        {
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
            _replyService = replyService;
            _friendshipService = friendshipService;
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Posts = await _postService.GetAllViewModelWithInclude();
            SavePostViewModel vm = new();
            return View(vm);
        }
    }
}
