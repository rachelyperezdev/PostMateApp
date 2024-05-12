using PostMateApp.Core.Application.ViewModels.Comment;
using PostMateApp.Core.Application.ViewModels.Reply;
using PostMateApp.Core.Application.ViewModels.User;

namespace PostMateApp.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Username { get; set; }
        public string? UserProfileImg { get; set; }
        public bool ToDelete { get; set; }

        public List<CommentViewModel>? Comments { get; set; }
        public List<ReplyViewModel>? Replies { get; set; }
        public UserViewModel? User { get; set; }
    }
}
