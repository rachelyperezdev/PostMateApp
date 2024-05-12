using PostMateApp.Core.Application.ViewModels.Reply;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IReplyService : IGenericService<SaveReplyViewModel, ReplyViewModel, Reply>
    {
    }
}
