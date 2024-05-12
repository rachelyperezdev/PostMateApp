using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Comment;
using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Core.Application.ViewModels.Reply;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Post>, IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IUserService _userService;

        public PostService(IPostRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(repository, mapper) 
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            vm.UserId = _userViewModel.Id;

            return await base.Add(vm);
        }

        public override async Task Update(SavePostViewModel vm, int id)
        {
            vm.UserId = _userViewModel.Id;

            await base.Update(vm, id);
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var postList = await _repository.GetAllWithIncludeAsync(new List<string> { "Comments", "Comments.Replies" });
            var user = await _userService.GetUserByIdAsync(_userViewModel.Id);
            var profileImg = user.ProfileImg;

            var postViewModels = new List<PostViewModel>();

            foreach (var post in postList.Where(post => post.UserId == _userViewModel.Id))
            {
                var comments = new List<CommentViewModel>();

                foreach (var comment in post.Comments ?? Enumerable.Empty<Comment>())
                {
                    var commentUser = await _userService.GetUserByIdAsync(comment.UserId);
                    var commentUserProfileImg = commentUser.ProfileImg;

                    var replies = new List<ReplyViewModel>();

                    foreach (var reply in comment.Replies ?? Enumerable.Empty<Reply>())
                    {
                        var replyUser = await _userService.GetUserByIdAsync(reply.UserId);
                        var replyUserProfileImg = replyUser.ProfileImg;

                        replies.Add(new ReplyViewModel
                        {
                            Id = reply.Id,
                            Text = reply.Text,
                            Username = replyUser.Username,
                            UserProfileImg = replyUserProfileImg,
                            CommentId = reply.CommentId,
                            PublicationDate = reply.PublicationDate,
                        });
                    }

                    comments.Add(new CommentViewModel
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        Username = commentUser.Username,
                        UserProfileImg = commentUserProfileImg,
                        Replies = replies,
                        PublicationDate = comment.PublicationDate,
                    });
                }

                postViewModels.Add(new PostViewModel
                {
                    Id = post.Id,
                    Username = _userViewModel.Username,
                    UserProfileImg = profileImg,
                    Text = post.Text,
                    ImageUrl = post.ImageUrl,
                    VideoUrl = post.VideoUrl,
                    PublicationDate = post.PublicationDate,
                    Comments = comments
                });
            }

            return postViewModels.OrderByDescending(post => post.PublicationDate).ToList();
        }

        public async Task<List<PostViewModel>> GetAllFriendsPosts()
        {
            string userId = _userViewModel.Id;
            var user = await _userService.GetUserByIdAsync(_userViewModel.Id);
            var profileImg = user.ProfileImg;
            var friendsPosts = await _repository.GetAllFriendsPostsAsync(userId, new List<string> { "Comments", "Comments.Replies" });

            var postViewModels = new List<PostViewModel>();

            foreach (var post in friendsPosts)
            {
                var postUser = await _userService.GetUserByIdAsync(post.UserId);
                var postUserProfileImg = postUser.ProfileImg;
                var comments = new List<CommentViewModel>();

                foreach (var comment in post.Comments ?? Enumerable.Empty<Comment>())
                {
                    var commentUser = await _userService.GetUserByIdAsync(comment.UserId);
                    var commentUserProfileImg = commentUser.ProfileImg;

                    var replies = new List<ReplyViewModel>();

                    foreach (var reply in comment.Replies ?? Enumerable.Empty<Reply>())
                    {
                        var replyUser = await _userService.GetUserByIdAsync(reply.UserId);
                        var replyUserProfileImg = replyUser.ProfileImg;

                        replies.Add(new ReplyViewModel
                        {
                            Id = reply.Id,
                            Text = reply.Text,
                            Username = replyUser.Username,
                            UserProfileImg = replyUserProfileImg,
                            CommentId = reply.CommentId,
                            PublicationDate = reply.PublicationDate,
                        });
                    }

                    comments.Add(new CommentViewModel
                    {
                        Id = comment.Id,
                        Text = comment.Text,
                        Username = commentUser.Username,
                        UserProfileImg = commentUserProfileImg,
                        Replies = replies,
                        PublicationDate = comment.PublicationDate,
                    });
                }

                postViewModels.Add(new PostViewModel
                {
                    Id = post.Id,
                    Username = postUser.Username,
                    UserProfileImg = postUserProfileImg,
                    Text = post.Text,
                    ImageUrl = post.ImageUrl,
                    VideoUrl = post.VideoUrl,
                    PublicationDate = post.PublicationDate,
                    Comments = comments
                });
            }

            return postViewModels.OrderByDescending(post => post.PublicationDate).ToList();
        }

        public async Task<PostViewModel> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            PostViewModel postVm = _mapper.Map<PostViewModel>(entity);
            return postVm;
        }

        public async Task<bool> IsPostOwner(string postId)
        {
            var post = await _repository.GetByIdAsync(int.Parse(postId));

            return post.UserId == _userViewModel.Id;
        }
    }
}
