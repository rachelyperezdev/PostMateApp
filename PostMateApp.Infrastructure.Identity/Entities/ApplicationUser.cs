using Microsoft.AspNetCore.Identity;

namespace PostMateApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? ProfileImg { get; set; }
    }
}
