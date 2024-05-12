using PostMateApp.Core.Application.ViewModels.Friendship;
using PostMateApp.Core.Application.ViewModels.User;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IFriendshipService : IGenericService<SaveFriendshipViewModel, FriendshipViewModel, FriendshipViewModel>
    {
        Task<string> AddFriend(string friendUsername);
        Task DeleteFriendship(string friendId);
        Task<List<UserViewModel>> GetFriends();
    }
}
