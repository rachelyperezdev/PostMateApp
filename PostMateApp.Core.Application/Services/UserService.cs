using AutoMapper;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.User;

namespace PostMateApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _accountService.GetAllUsersAsync();
            
            List<UserViewModel> usersVm = _mapper.Map<List<UserViewModel>>(users);

            return usersVm;
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            var user = await _accountService.GetUserByUsernameAsync(username);

            UserViewModel userVm = _mapper.Map<UserViewModel>(user);
            return userVm;
        }

        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            var user = await _accountService.GetUserByIdAsync(userId);

            UserViewModel userVm = _mapper.Map<UserViewModel>(user);
            return userVm;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            return await _accountService.AuthenticateAsync(loginRequest);
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterUserAsync(registerRequest, origin);
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetPasswordRequest);
        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserViewModel vm) 
        {
            UpdateUserRequest request = _mapper.Map<UpdateUserRequest>(vm);
            return await _accountService.UpdateUserAsync(request);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _accountService.UsernameExists(username);
        }

    }
}
