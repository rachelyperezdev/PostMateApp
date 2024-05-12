using Microsoft.AspNetCore.Identity;
using PostMateApp.Infrastructure.Identity.Entities;
using System.Data;
using PostMateApp.Core.Application.Enums;

namespace PostMateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.ProfileOwner.ToString()));
        }
    }
}
