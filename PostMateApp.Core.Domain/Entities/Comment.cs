using PostMateApp.Core.Domain.Common;

namespace PostMateApp.Core.Domain.Entities
{
    public class Comment : BaseComment
    {
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
