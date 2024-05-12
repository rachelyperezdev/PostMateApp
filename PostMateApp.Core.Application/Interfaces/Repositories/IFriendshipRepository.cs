using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Interfaces.Repositories
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task DeleteFriendship(string profileOwnerId, string friendId);
        Task<List<string>> GetFriendIdsAsync(string userId);
    }
}
