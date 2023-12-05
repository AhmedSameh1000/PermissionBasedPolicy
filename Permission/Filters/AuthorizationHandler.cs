using Microsoft.AspNetCore.Authorization;
using Permission.Constant;

namespace Permission.Filters
{
    public class AuthorizationHandler : AuthorizationHandler<PermissionRequirments>
    {
        public AuthorizationHandler()
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirments requirement)
        {
            if (context.User == null)
            {
                return;
            }

            var canAccsess = context.User.Claims.Any(c => c.Type == Permissions.Products.Permissions && c.Value == requirement.Permission);

            if (canAccsess)
            {
                context.Succeed(requirement);
                return;
            }
            else
            {
                context.Fail();
            }
        }
    }
}