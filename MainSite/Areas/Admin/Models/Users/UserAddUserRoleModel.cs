using System.Collections.Concurrent;
using System.Collections.Generic;
using MainSite.Areas.Admin.Models.Security;

namespace MainSite.Areas.Admin.Models.Users
{
    public class UserAddUserRoleModel
    {
        public UserAddUserRoleModel()
        {
            AvailableUserRoles = new List<UserRoleModel>();
            Allowed = new Dictionary<string, bool>();
        }
        public UserAddUserRoleModel(string userId) : this()
        {
            UserId = userId;
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string SystemName { get; set; }

        public IList<UserRoleModel> AvailableUserRoles { get; set; }
        // [customer role id] / [allowed]
        public IDictionary<string, bool> Allowed { get; set; }

    }
}