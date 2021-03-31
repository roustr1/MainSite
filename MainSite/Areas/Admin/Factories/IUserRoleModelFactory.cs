using System.Collections.Generic;
using Application.Dal.Domain.Users;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Factories
{

    /// <summary>
    /// Represents the customer role model factory
    /// </summary>
    public partial interface IUserRoleModelFactory
    {
        /// <summary>
        /// Prepare customer role model
        /// </summary>
        /// <param name="model">User role model</param>
        /// <param name="UserRole">User role</param>
        /// <returns>User role model</returns>
        UserRoleModel PrepareUserRoleModel(UserRoleModel model, UserRole UserRole);
    }

}