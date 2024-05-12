using Microsoft.EntityFrameworkCore;
using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Domain.Entities;
using PostMateApp.Infrastructure.Persistence.Contexts;

namespace PostMateApp.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationContext _dbContext;

        public FriendshipRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetFriendIdsAsync(string userId)
        {
            var friends = await _dbContext.Set<Friendship>()
                .Where(f => f.ProfileOwnerId == userId)
                .Select(f => f.FriendId)
                .ToListAsync();
            return friends;
        }

        public async Task DeleteFriendship(string profileOwnerId, string friendId)
        {
            var friendship = await _dbContext.Set<Friendship>()
                                     .FirstOrDefaultAsync(friendship => 
                                     friendship.ProfileOwnerId == profileOwnerId &&  friendship.FriendId == friendId);

            if (friendship != null)
            {
                _dbContext.Set<Friendship>().Remove(friendship);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
