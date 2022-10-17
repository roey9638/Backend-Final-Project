using Microsoft.AspNetCore.Identity;

namespace Reactivities.Modules
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}
