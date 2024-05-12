using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Core.Application.Interfaces.Repositories;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Friendship;
using PostMateApp.Core.Application.ViewModels.User;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Core.Application.Services
{
    public class FriendshipService : GenericService<SaveFriendshipViewModel, FriendshipViewModel, Friendship>, IFriendshipService
    {
        private readonly IFriendshipRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly IAccountService _accountService;

        public FriendshipService(IFriendshipRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IAccountService accountService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<List<UserViewModel>> GetFriends()
        {
            var friendsIds = await _repository.GetFriendIdsAsync(_userViewModel.Id);

            var friendsDTOs = await _accountService.GetFriends(friendsIds);

            List<UserViewModel> friends = _mapper.Map<List<UserViewModel>>(friendsDTOs);

            return friends;
        }

        public async Task<string> AddFriend(string friendUsername)
        {
            var userExists = await _accountService.GetUserByUsernameAsync(friendUsername);

            if(userExists != null) 
            {
                if(userExists.Id != _userViewModel.Id)
                {
                    var friendship = new Friendship
                    {
                        ProfileOwnerId = _userViewModel.Id,
                        FriendId = userExists.Id
                    };

                    await _repository.AddAsync(friendship);
                    return $"{friendUsername} ha sido agregado a su lista de amigos.";
                }
                else
                {
                    return $"{friendUsername}, no puede agregarse así mismo como amigo.";
                }
            }
            else
            {
                return $"{friendUsername} no ha sido encontrado.";
            }
        }

        public async Task DeleteFriendship(string friendId)
        {
            await _repository.DeleteFriendship(_userViewModel.Id, friendId);
        }
    }
}
