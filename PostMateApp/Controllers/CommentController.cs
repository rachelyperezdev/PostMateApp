using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Comment;

namespace PostMateApp.Controllers
{
    public class CommentController : Controller
    {
       private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string text, int postId, string redirectController)
        {

            SaveCommentViewModel vm = new()
            {
                Text = text,
                PostId = postId,
                PublicationDate = DateTime.Now,
            };

            await _commentService.Add(vm);

            return RedirectToRoute(new { controller=redirectController , action="Index" });
        }


    }
}
