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
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };


        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel

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

                    }
                ),
                (
                    AppUserDefaults.ForumModeratorsRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                    }
                ),

                (
                    AppUserDefaults.RegisteredRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                    }
                )
            };
        }
    }

}