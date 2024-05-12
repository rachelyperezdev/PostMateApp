using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Domain.Common
{
    public class BaseComment : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
