using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Permission.DTOs;

namespace Permission.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;

        public UsersController(UserManager<IdentityUser> userManager
        , RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Mange(string id)
        {
            var UserSelected = await _userManager.FindByIdAsync(id);
            if (UserSelected is null) return NotFound();

            var Roles = await _roleManager.Roles.ToListAsync();

            var ViewModel = new UserRolesViewModel()
            {
                UserId = UserSelected.Id,
                UserName = UserSelected.Email.Split('@')[0],
                Roles = Roles.Select(Role => new RolesViewModels()
                {
                    RoleId = Role.Id,
                    RoleName = Role.Name,
                    IsSelected = _userManager.IsInRoleAsync(UserSelected, Role.Name).Result
                }).ToList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mange(UserRolesViewModel model)
        {
            var UserSelected = await _userManager.FindByIdAsync(model.UserId);
            if (UserSelected is null) return NotFound();

            var UserRoles = await _userManager.GetRolesAsync(UserSelected);
            //foreach (var Role in model.Roles)
            //{
            //    if (UserRoles.Any(r => r == Role.RoleName) && !Role.IsSelected)
            //        await _userManager.RemoveFromRoleAsync(UserSelected, Role.RoleName);

            //    if (!UserRoles.Any(r => r == Role.RoleName) && Role.IsSelected)
            //        await _userManager.AddToRoleAsync(UserSelected, Role.RoleName);
            //}
            await _userManager.RemoveFromRolesAsync(UserSelected, UserRoles);
            await _userManager.AddToRolesAsync(UserSelected, model.Roles.Where(c => c.IsSelected).Select(c => c.RoleName));

            return RedirectToAction(nameof(Index));
        }
    }
}