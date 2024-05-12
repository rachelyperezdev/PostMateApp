using Microsoft.AspNetCore.Mvc;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.ViewModels.Post;
using PostMateApp.Core.Application.Helpers;
using PostMateApp.Middlewares;

namespace PostMateApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTextPost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveTextPost", vm);
            }

            await _postService.Add(vm);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateImagePost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImagePost", vm);
            }

            SavePostViewModel postVm = await _postService.Add(vm);

            if (postVm.Id != 0 && postVm != null)
            {
                postVm.ImageUrl = UploadFile(vm.File, postVm.Id);
                await _postService.Update(postVm, postVm.Id);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideoPost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImagePost", vm);
            }

            vm.VideoUrl = YoutubeHelper.ExtractIntegrationLink(vm.VideoUrl);

            await _postService.Add(vm);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> EditTextPost(int id)
        {
            SavePostViewModel vm = await _postService.GetByIdSaveViewModel(id);
            return View("SaveTextPost", vm);
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        [HttpPost]
        public async Task<IActionResult> EditTextPost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveTextPost", vm);
            }

            await _postService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> EditImagePost(int id)
        {
            SavePostViewModel vm = await _postService.GetByIdSaveViewModel(id);
            return View("SaveImagePost", vm);
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        [HttpPost]
        public async Task<IActionResult> EditImagePost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImagePost", vm);
            }

            SavePostViewModel postVm = await _postService.GetByIdSaveViewModel(vm.Id);
            vm.ImageUrl = UploadFile(vm.File, vm.Id, true, postVm.ImageUrl);
            await _postService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [ServiceFilter(typeof(ProfileOwnerAuthorize))]
        public async Task<IActionResult> EditVideoPost(int id)
        {
            SavePostViewModel vm = await _postService.GetByIdSaveViewModel(id);
            return View("SaveVideoPost", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditVideoPost(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveVideoPost", vm);
            }

            await _postService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.Delete(id);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = " ")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }

            string basePath = $"/Images/Posts/{id}";
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
