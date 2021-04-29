using System.Collections.Generic;
using Application.Dal.Domain.Permissions;
using Application.Dal.Domain.Users;

namespace Application.Services.Permissions
{

    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //permission example
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord ManageAcl = new PermissionRecord { Name = "Admin area. Manage ACL", SystemName = "ManageACL", Category = "Configuration" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage users", SystemName = "ManageUsers", Category = "Users" };
        public static readonly PermissionRecord ManageMenu = new PermissionRecord { Name = "Admin area. Manage menu", SystemName = "ManageMenu", Category = "Standart" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Admin area. Manage settings", SystemName = "ManageSettings", Category = "Standart" };
        public static readonly PermissionRecord AccessToIndexPage = new PermissionRecord { Name = "User area. Access to common page", SystemName = "AccessToIndex", Category = "Standart" };
        public static readonly PermissionRecord EditNews = new PermissionRecord { Name = "User area. CRUD operations in news", SystemName = "EditNews", Category = "Standart" };

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                ManageAcl,
                ManageUsers,
                ManageMenu,
                ManageSettings,AccessToIndexPage,EditNews
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    AppUserDefaults.AdministratorsRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                        ManageAcl,
                        ManageUsers,
                        ManageMenu,
                        ManageSettings,
                        AccessToIndexPage,
                        EditNews

                    }
                ),
                (
                    AppUserDefaults.ModeratorsRoleName,
                    new[]
                    {
                        AccessAdminPanel,AccessToIndexPage,EditNews
                    }
                ),

                (
                    AppUserDefaults.RegisteredRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                        AccessToIndexPage
                    }
                )
            };
        }
    }

}