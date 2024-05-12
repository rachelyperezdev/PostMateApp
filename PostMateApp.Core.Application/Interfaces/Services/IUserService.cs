using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.ViewModels.User;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task SignOutAsync();
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task<bool> UsernameExists(string username);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<UserViewModel> GetUserByIdAsync(string userId);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserViewModel vm);
    }
}
