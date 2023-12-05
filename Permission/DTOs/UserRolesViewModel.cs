using System.ComponentModel.DataAnnotations;

namespace Permission.DTOs
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public List<RolesViewModels> Roles { get; set; }
    }

    public class PermissionFormViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<RolesViewModels> RoleClaims { get; set; }
    }

    public class RolesViewModels
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }

    public class RoleFormViewModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}