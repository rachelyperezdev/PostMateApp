using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Domain.Entities;
using PostMateApp.Infrastructure.Persistence.Contexts;

namespace PostMateApp.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly ApplicationContext _dbContext;

        public CommentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
