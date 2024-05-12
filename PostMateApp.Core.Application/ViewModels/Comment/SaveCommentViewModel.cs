using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Core.Application.ViewModels.Reply;
using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public PostViewModel? Post { get; set; }
        public ICollection<ReplyViewModel>? Replies { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Debe escribir algo para enviar el comentario.")]
        public string Text { get; set; }
        [Required(ErrorMessage = "La fecha de publicación es necesaria.")]
        public DateTime PublicationDate { get; set; }
    }
}
