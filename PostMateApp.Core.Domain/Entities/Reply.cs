using PostMateApp.Core.Domain.Common;

namespace PostMateApp.Core.Domain.Entities
{
    public class Reply : BaseComment
    {
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
