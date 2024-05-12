using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Reply;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Services
{
    public class ReplyService : GenericService<SaveReplyViewModel, ReplyViewModel, Reply>, IReplyService
    {
        private readonly IReplyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public ReplyService(IReplyRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public override async Task<SaveReplyViewModel> Add(SaveReplyViewModel vm)
        {
            vm.UserId = _userViewModel.Id;
            return await base.Add(vm);
        }

        public override async Task Update(SaveReplyViewModel vm, int id)
        {
            vm.UserId = _userViewModel.Id;
            await base.Update(vm, id);
        }
    }
}
