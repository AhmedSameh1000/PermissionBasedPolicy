using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Permission.Constant;
using Permission.DTOs;
using System.Security.Claims;

namespace Permission.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();

            return View(Roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", await _roleManager.Roles.ToListAsync());

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is exists!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MangePermission(string RoleId)
        {
            var Role = await _roleManager.FindByIdAsync(RoleId);
            if (Role is null)
            {
                return NotFound();
            }

            var RoleCalims = _roleManager.GetClaimsAsync(Role).Result.Select(c => c.Value).ToList();

            var AllClaims = Permissions.GenerateAllPermissions();

            var AllPermission = AllClaims.Select(p => new RolesViewModels
            {
                RoleName = p
            }).ToList();

            foreach (var p in AllPermission)
            {
                if (RoleCalims.Any(c => c == p.RoleName))
                    p.IsSelected = true;
            }

            var ViewModel = new PermissionFormViewModel()
            {
                RoleId = RoleId,
                RoleName = Role.Name,
                RoleClaims = AllPermission
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MangePermission(PermissionFormViewModel model)
        {
            var Role = await _roleManager.FindByIdAsync(model.RoleId);
            if (Role is null)
            {
                return NotFound();
            }

            var RoleCalims = await _roleManager.GetClaimsAsync(Role);

            foreach (var Clim in RoleCalims)
            {
                await _roleManager.RemoveClaimAsync(Role, Clim);
            }
            var SelectedClaims = model.RoleClaims.Where(c => c.IsSelected).ToList();

            foreach (var Selectedclaim in SelectedClaims)
            {
                await _roleManager.AddClaimAsync(Role, new Claim(Permissions.Products.Permissions, Selectedclaim.RoleName));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}