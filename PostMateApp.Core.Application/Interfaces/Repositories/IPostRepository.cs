using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllFriendsPostsAsync(string currentUserId, List<string> properties);
        Task<Post> GetById(int id);
    }
}
