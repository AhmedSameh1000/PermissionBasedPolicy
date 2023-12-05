using Microsoft.AspNetCore.Authorization;

namespace Permission.Filters
{
    public class PermissionRequirments : IAuthorizationRequirement
    {
        public PermissionRequirments(string Permission)
        {
            this.Permission = Permission;
        }

        public string Permission { get; }
    }
}