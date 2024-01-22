using Microsoft.AspNetCore.Identity;

namespace Commerce.ViewModels
{
    public class RolesViewModel
    {
        public IdentityRole Role { get; set; }
        public ICollection<IdentityRole> Roles { get; set; }
    }
}
