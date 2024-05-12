using PostMateApp.Core.Application.ViewModels.Comment;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<SaveCommentViewModel, CommentViewModel, Comment>
    {
    }
}
