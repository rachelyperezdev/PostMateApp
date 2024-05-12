using PostMateApp.Core.Domain.Common;

namespace PostMateApp.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime PublicationDate { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
