using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Permission.Constant;
using System.Security.Claims;

namespace Permission.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            }
        }
    }

    public static class DefaultUsers
    {
        public static async Task SeedBasicAsync(UserManager<IdentityUser> userManager)
        {
            var Defaultuser = new IdentityUser()
            {
                Email = "Basic@gmail.com",
                UserName = "Basic@gmail.com",
                EmailConfirmed = true,
            };
            var User = await userManager.FindByEmailAsync(Defaultuser.Email);

            if (User == null)
            {
                await userManager.CreateAsync(Defaultuser, "ahmeds1490");

                await userManager.AddToRoleAsync(Defaultuser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var Defaultuser = new IdentityUser()
            {
                Email = "SuperAdmin@gmail.com",
                UserName = "SuperAdmin@gmail.com",
                EmailConfirmed = true,
            };
            var User = await userManager.FindByEmailAsync(Defaultuser.Email);

            if (User == null)
            {
                await userManager.CreateAsync(Defaultuser, "ahmeds1490");

                await userManager.AddToRolesAsync(Defaultuser, new List<string> { Roles.Basic.ToString(),
                    Roles.Admin.ToString(), Roles.SuperAdmin.ToString() });
            }
            await roleManager.SeedClaimsForSuperAdminRole();
        }

        private static async Task SeedClaimsForSuperAdminRole(this RoleManager<IdentityRole> roleManager)
        {
            var SuperAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

            await roleManager.AddPermissionClaimsToSpasificRole(SuperAdminRole, "Products");
        }

        public static async Task AddPermissionClaimsToSpasificRole(this RoleManager<IdentityRole> roleManager, IdentityRole role, string Module)
        {
            var AllCalims = await roleManager.GetClaimsAsync(role);

            var Allpermissions = Permissions.GeneratePermissionsList(Module);

            foreach (var Permission in Allpermissions)
            {
                if (!AllCalims.Any(c => c.Type == Permissions.Products.Permissions && c.Value == Permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(Permissions.Products.Permissions, Permission));
                }
            }
        }
    }
}