using PostMateApp.Core.Application.ViewModels.Comment;
using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.Reply
{
    public class SaveReplyViewModel
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public CommentViewModel? Comment { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        [Required(ErrorMessage = "Debe establecer la fecha de publicación.")]
        public DateTime PublicationDate { get; set; }
    }
}
