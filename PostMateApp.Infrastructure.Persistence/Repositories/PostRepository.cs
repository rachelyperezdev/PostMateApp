using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Domain.Entities;
using PostMateApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PostMateApp.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationContext _dbContext;

        public PostRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllFriendsPostsAsync(string currentUserId, List<string> properties)
        {
            var postList = await GetAllWithIncludeAsync(properties);

            var filteredPostList = postList.Where(post => _dbContext.Set<Friendship>()
                                .Any(friendship => friendship.ProfileOwnerId == currentUserId && friendship.FriendId == post.UserId));

            return filteredPostList.ToList();
        }

        public async Task<Post> GetById(int id)
        {
            return await _dbContext.Set<Post>().FindAsync(id);
        }

    }
}
