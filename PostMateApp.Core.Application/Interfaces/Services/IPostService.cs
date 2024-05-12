using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
        Task<List<PostViewModel>> GetAllFriendsPosts();
        Task<PostViewModel> GetById(int id);
        Task<bool> IsPostOwner(string postId);
    }
}
