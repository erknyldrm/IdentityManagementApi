using Microsoft.AspNetCore.Identity;

namespace IdentityManagement.Infrastructure.Persistence
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
