using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Permission.Constant;

namespace Permission.Filters
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FullBackpolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FullBackpolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()

        {
            return FullBackpolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return FullBackpolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(Permissions.Products.Permissions, StringComparison.OrdinalIgnoreCase))
            {
                var Policy = new AuthorizationPolicyBuilder();
                Policy.AddRequirements(new PermissionRequirments(policyName));

                return Task.FromResult(Policy.Build());
            }
            return FullBackpolicyProvider.GetPolicyAsync(policyName);
        }
    }
}