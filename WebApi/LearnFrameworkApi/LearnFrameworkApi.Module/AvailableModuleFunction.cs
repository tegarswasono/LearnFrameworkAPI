namespace LearnFrameworkApi.Module
{
    public static class AvailableModuleFunction
    {
        public static List<ModuleFunctionModel> GetAll()
        {
#pragma warning disable CS8601 // Possible null reference assignment.

            int a = 1;
            var result = typeof(AvailableModuleFunction)
                .GetFields()
                .Select(x => new ModuleFunctionModel 
                { 
                    //Id = x.Name, 
                    Id = x.GetValue(null)?.ToString(),
					IdText = x.GetValue(null)?.ToString(),
                    Module = x.GetValue(null)?.ToString()?.Split(".")[0],
                    FunctionName = x.GetValue(null)?.ToString()?.Split(".")[1],
                    Order = a++
                }).ToList();
#pragma warning restore CS8601 // Possible null reference assignment.
            return result;
        }

        public const string SMTPSettingView = "SMTPSetting.View";
        public const string SMTPSettingCreateOrUpdate = "SMTPSetting.CreateOrUpdate";
        public const string SystemConfigurationView = "SystemConfiguration.View";
        public const string SystemConfigurationCreateOrUpdate = "SystemConfiguration.CreateOrUpdate";

        public const string UsersView = "Users.View";
        public const string UsersAdd = "Users.Add";
        public const string UsersEdit = "Users.Edit";
        public const string UsersDelete = "Users.Delete";

		public const string RolesView = "Roles.View";
		public const string RolesAdd = "Roles.Add";
		public const string RolesEdit = "Roles.Edit";
		public const string RolesDelete = "Roles.Delete";

		public const string CategoryView = "Category.View";
		public const string CategoryAdd = "Category.Add";
		public const string CategoryEdit = "Category.Edit";
		public const string CategoryDelete = "Category.Delete";

		public const string ProductView = "Product.View";
		public const string ProductAdd = "Product.Add";
		public const string ProductEdit = "Product.Edit";
		public const string ProductDelete = "Product.Delete";

		public const string BookingsView = "Bookings.View";

    }
    public class ModuleFunctionModel
    {
        public string Id { get; set; } = string.Empty;
        public string IdText { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string FunctionName { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
