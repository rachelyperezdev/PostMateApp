using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using PostMateApp.Middlewares;

namespace PostMateApp.Controllers
{
    
    public class FriendController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReplyService _replyService;
        private readonly IFriendshipService _friendshipService; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendController(IUserService userService, IPostService postService, ICommentService commentService, IReplyService replyService, IFriendshipService friendshipService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
            _replyService = replyService;
            _friendshipService = friendshipService;
            _httpContextAccessor = httpContextAccessor;
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Friends = await _friendshipService.GetFriends();
            ViewBag.FriendsPosts = await _postService.GetAllFriendsPosts();
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddFriend()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string username)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Por favor, introduce un nombre de usuario.");
                return View();
            }

            string addFriendResponse = await _friendshipService.AddFriend(username);    

            if(addFriendResponse != $"{username} ha sido agregado a su lista de amigos.")
            {
                ModelState.AddModelError("username", $"{addFriendResponse}");
                return View();
            }

            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _friendshipService.DeleteFriendship(id);
            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }
    }
}
