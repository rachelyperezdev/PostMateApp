using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Comment;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Services
{
    public class CommentService : GenericService<SaveCommentViewModel, CommentViewModel, Comment>, ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public CommentService(ICommentRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper) 
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SaveCommentViewModel> Add(SaveCommentViewModel vm)
        {
            vm.UserId = _userViewModel.Id;
            return await base.Add(vm);
        }

        public override async Task Update(SaveCommentViewModel vm, int id)
        {
            vm.UserId = _userViewModel.Id;
            await base.Update(vm, id);
        }
    }
}
