using Commerce.Data;
using Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Controllers.Admin
{
    public class UsersManagerController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersManagerController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public ActionResult Index()
        {
            var users = userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName,
                Roles = userManager.GetRolesAsync(u).Result.ToList()
            }).ToList();

            return View(users);
        }

        public async Task<IActionResult> Search(string searchKey)
        {
            var users = await context.Users
                .Where(s => s.UserName.Contains(searchKey)).Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserName = u.UserName,
                    Roles = userManager.GetRolesAsync(u).Result.ToList()
                }).ToListAsync();


            return View("Index", users);
        }

        public async Task<ActionResult> Manage(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            var userVM = new UserRolesViewModel
            {
                Id = id,
                UserName = user.UserName,
                roleManager = roleManager.Roles.Select(r => new RoleManagerViewModel
                {
                    Id = r.Id,
                    Role = r.Name,
                    IsSelected = userManager.IsInRoleAsync(user, r.Name).Result

                }).ToList()
            };

            return View(userVM);
        }

        // POST: ManageUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(UserRolesViewModel userRoles)
        {
            var user = await userManager.FindByNameAsync(userRoles.UserName);

            var roles = new List<string>();

            foreach (var item in userRoles.roleManager)
            {
                if (item.IsSelected)
                {
                    roles.Add(item.Role);
                }
            }

            await userManager.RemoveFromRolesAsync(user, userManager.GetRolesAsync(user).Result.ToList());

            await userManager.AddToRolesAsync(user, roles);

            return RedirectToAction(nameof(Index));
        }
    }
}
