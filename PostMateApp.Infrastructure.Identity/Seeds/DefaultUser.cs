using Microsoft.AspNetCore.Identity;
using PostMateApp.Core.Application.Enums;
using PostMateApp.Infrastructure.Identity.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace PostMateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "basicUser";
            defaultUser.Email = "basicuser@gmail.com";
            defaultUser.Firstname = "Jane";
            defaultUser.Lastname = "Smith";
            defaultUser.PhoneNumber = "809-111-1111";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.ProfileImg = "/Images/ProfileOwner/basicUser/basicUser.png";

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);

                if(user == null)
                {
                    await userManager.CreateAsync(defaultUser, "987Pa$$wOrD**");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProfileOwner.ToString());
                }
            }
        }
    }
}
