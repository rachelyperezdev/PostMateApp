using PostMateApp.Core.Application.ViewModels.Comment;

namespace PostMateApp.Core.Application.ViewModels.Reply
{
    public class ReplyViewModel
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public CommentViewModel? Comment { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string? UserProfileImg { get; set; }
        public string Text { get; set; }
        public string RedirectController { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
