using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Reply;

namespace PostMateApp.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IReplyService _replyService;

        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string text, int commentId, string redirectController)
        {
            SaveReplyViewModel vm = new()
            {
                Text = text,
                CommentId = commentId,
                PublicationDate = DateTime.Now,
            };

            await _replyService.Add(vm);

            return RedirectToRoute(new { controller = redirectController, action = "Index" });
        }

    }
}
