using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PostMateApp.Infrastructure.Identity.Entities;
using PostMateApp.Infrastructure.Identity.Seeds;

namespace PostMateApp.Infrastructure.Identity
{
    public static class ServiceApplication
    {
        public static async Task AddIdentitySeeds(this IServiceProvider services)
        {
            #region "Identity Seeds"
            using (var scope = services.CreateScope())
            {
                var serviceScope = scope.ServiceProvider;

                try
                {
                    var userManager = serviceScope.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceScope.GetRequiredService<RoleManager<IdentityRole>>();


                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultUser.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
        }
    }
}
