using Microsoft.AspNetCore.Identity;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Infrastructure.Identity.Entities;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PostMateApp.Core.Application.Enums;

namespace PostMateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No hay cuentas registradas con '{request.Username}'";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Cuenta no confirmada para '{request.Username}'. Diríjase a su correo y active su cuenta.";
                return response;
            }

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Credenciales inválidas para '{request.Username}'";
                return response;
            }
            

            response.Id = user.Id;
            response.Email = user.Email;
            response.Username = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return $"No hay cuentas registradas para este usuario.";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return $"Cuenta confirmada para '{user.UserName}'. \nBienvenido a PostMate!";
            }
            else
            {
                return $"Ha ocurrido un error confirmando la cuenta de '{user.UserName}'";
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Password = user.PasswordHash,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ProfileImg = user.ProfileImg
            }).ToList();

            return userDTOs;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            UserDTO userDTO = new()
            {
                Id = user.Id,
                Username = user.UserName,
                Password = user.PasswordHash,
                Firstname = user.Firstname, 
                Lastname = user.Lastname,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ProfileImg = user.ProfileImg
            };

            return userDTO;
        }

        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            UserDTO userDTO = new()
            {
                Id = user.Id,
                Username = user.UserName,
                Password = user.PasswordHash,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Phone = user.PhoneNumber,
                Email = user.Email,
                ProfileImg = user.ProfileImg
            };

            return userDTO;
        }

        public async Task<List<UserDTO>> GetFriends(List<string> friendsIds)
        {
            var friends = await _userManager.Users
                .Where(u => friendsIds.Contains(u.Id))
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Username = u.UserName,
                    ProfileImg = u.ProfileImg
                })
                .ToListAsync();

            return friends;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new RegisterResponse()
            {
                HasError = false
            };

            var userWithUserName = await _userManager.FindByNameAsync(request.Username);
            if (userWithUserName != null) 
            { 
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.Username}' ya está registrado.";
                return response;
            }

            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithEmail != null)
            {
                response.HasError = true;
                response.Error = $"El email '{request.Email}' ya está registrado.";
                return response;
            }

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                ProfileImg = request.ProfileImg
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.ProfileOwner.ToString());
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new Core.Application.DTOs.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"Casi eres parte de PostMate.\nPara terminar, por favor confirma tu cuenta a través de tu correo visitando esta URL {verificationUri}",
                    Subject = $"Confirmación de Registro de Cuenta"
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"Un error ocurrió mientras se registraba la cuenta.";
                return response;
            }

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false,
            };

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                response.HasError= true;
                response.Error = $"No hay cuentas registradas con el nombre de usuario '{request.Username}'";
                return response;
            }

            var generatedPassword = GeneratePassword.GenerateRandomPassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            request.Token = token;
            var result = await _userManager.ResetPasswordAsync(user, request.Token, generatedPassword);

            if(!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Un error ocurrió mientras se reseteaba la contraseña.";
                return response;
            }

            await _emailService.SendAsync(new Core.Application.DTOs.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Su contraseña ha sido reseteada. Su nueva contraseña es {generatedPassword}",
                Subject = "Nueva Contraseña"
            });

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> UsernameExists(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = $"No se encontraron usuarios con el ID: {request.Id}"
                };
            }

            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.PhoneNumber = request.Phone;
            user.Email = request.Email;
            user.ProfileImg = request.ProfileImg;

            if (request.Password != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = $"Error actualizando la información del usuario con el ID: {request.Id}"
                };
            }

            return new UpdateUserResponse();
        }

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Account/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        
    }
}
