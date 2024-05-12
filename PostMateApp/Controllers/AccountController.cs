using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.DTOs.Account;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.User;
using PostMateApp.Middlewares;
using PostMateApp.Core.Application.Helpers;

namespace PostMateApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            var errorMessage = HttpContext.Request.Cookies["ErrorMessage"];
            if (errorMessage != null)
            {
                HttpContext.Response.Cookies.Delete("ErrorMessage");
                ViewBag.ErrorMessage = errorMessage;
            }

            return View(new LoginViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller= "Account", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Register()
        {
            return View(new SaveUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            vm.ProfileImg = UploadFile(vm.File, vm.Username);
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Account", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "Account", action = "Index" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> Edit(string username, string id)
        {
            var errorMessage = HttpContext.Request.Cookies["ErrorMessage"];
            if (errorMessage != null)
            {
                HttpContext.Response.Cookies.Delete("ErrorMessage");
                ViewBag.ErrorMessage = errorMessage;
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            UpdateUserViewModel vm = new UpdateUserViewModel()
            {
                Id = id,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
                ProfileImg = user.ProfileImg
            };

            return View(vm);
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel vm)
        {
            if (vm.File == null)
            {
                var userVm = await _userService.GetUserByUsernameAsync(vm.Username);
                vm.ProfileImg = UploadFile(vm.File, vm.Username, true, userVm.ProfileImg);
            }
            else
            {
                vm.ProfileImg = UploadFile(vm.File, vm.Username, true, vm.ProfileImg);
            }


            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _userService.UpdateUserAsync(vm);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        private string UploadFile(IFormFile file, string username, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }

            string basePath = $"/Images/ProfileOwner/{username}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split('/');
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }
    }
}
