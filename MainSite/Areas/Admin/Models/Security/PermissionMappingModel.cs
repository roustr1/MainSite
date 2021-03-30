using System.Collections.Generic;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Models.Security
{
    /// <summary>
    /// Represents a permission mapping model
    /// </summary>
    public partial class PermissionMappingModel 
    {
        #region Ctor

        public PermissionMappingModel()
        {
            AvailablePermissions = new List<PermissionRecordModel>();
            AvailableUserRoles = new List<UserRoleModel>();
            Allowed = new Dictionary<string, IDictionary<string, bool>>();
        }

        #endregion

        #region Properties

        public IList<PermissionRecordModel> AvailablePermissions { get; set; }

        public IList<UserRoleModel> AvailableUserRoles { get; set; }

        //[permission system name] / [customer role id] / [allowed]
        public IDictionary<string, IDictionary<string, bool>> Allowed { get; set; }

        #endregion
    }
}