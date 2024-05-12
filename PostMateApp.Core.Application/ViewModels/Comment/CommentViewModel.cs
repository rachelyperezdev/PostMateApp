using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Core.Application.ViewModels.Reply;

namespace PostMateApp.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public PostViewModel? Post { get; set; }
        public List<ReplyViewModel>? Replies { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string? UserProfileImg { get; set; }
        public string Text { get; set; }
        public string RedirectController { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
