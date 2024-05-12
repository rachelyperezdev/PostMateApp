using PostMateApp.Core.Application.DTOs.Account;

namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task SignOutAsync();
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO?> GetUserByIdAsync(string username);
        Task<bool> UsernameExists(string username);
        Task<List<UserDTO>> GetFriends(List<string> friendsIds);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
    }
}
