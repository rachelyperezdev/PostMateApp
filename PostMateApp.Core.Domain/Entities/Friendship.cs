using PostMateApp.Core.Domain.Common;

namespace PostMateApp.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        public string ProfileOwnerId { get; set; }
        public string FriendId { get; set; }
    }
}
