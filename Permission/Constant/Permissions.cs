namespace Permission.Constant
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string Module)
        {
            return new List<string>()
            {
                $"Permissions.{Module}.View",
                $"Permissions.{Module}.Create",
                $"Permissions.{Module}.Edit",
                $"Permissions.{Module}.Delete",
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var Permissions = new List<string>();

            var Modules = Enum.GetValues(typeof(Modules));
            foreach (var Module in Modules)
                Permissions.AddRange(GeneratePermissionsList(Module.ToString()));

            return Permissions;
        }

        public static class Products
        {
            public const string Permissions = "Permissions";
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Delete = "Permissions.Products.Delete";
            public const string Edit = "Permissions.Products.Edit";
        }
    }
}