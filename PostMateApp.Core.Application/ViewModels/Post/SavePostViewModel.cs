using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.ViewModels.Comment;
using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        [DataType(DataType.MultilineText)]
        public string? Text { get; set; }

        [DataType(DataType.Text)]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        [DataType(DataType.Url)]
        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Debe establecer la fecha de publicación.")]
        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; }

        public ICollection<CommentViewModel>? Comments { get; set; }

        public int Id { get; set; }
        public string? UserId { get; set; }

    }
}
