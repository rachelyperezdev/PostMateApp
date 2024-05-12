using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Domain.Entities;
using PostMateApp.Infrastructure.Persistence.Contexts;

namespace PostMateApp.Infrastructure.Persistence.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        private readonly ApplicationContext _dbContext;

        public ReplyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
