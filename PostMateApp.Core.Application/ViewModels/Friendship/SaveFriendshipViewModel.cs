using System.ComponentModel.DataAnnotations;

namespace PostMateApp.Core.Application.ViewModels.Friendship
{
    public class SaveFriendshipViewModel
    {
        [Required]
        public string ProfileOwnerId { get; set; }
        [Required]
        public string FriendId { get; set; }
    }
}
