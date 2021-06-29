using System.Linq;
using Application.Dal.Domain.Users;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the customer role model factory implementation
    /// </summary>
    public partial class UserRoleModelFactory : IUserRoleModelFactory
    {
        #region Fields

        private readonly IUsersService _customerService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public UserRoleModelFactory(IUsersService customerService, IPermissionService permissionService)
        {
            _customerService = customerService;
            _permissionService = permissionService;
        }

        #endregion


        #region Methods

  
 
        /// <summary>
        /// Prepare customer role model
        /// </summary>
        /// <param name="model">User role model</param>
        /// <param name="userRole">User role</param>
        /// <returns>User role model</returns>
        public virtual UserRoleModel PrepareUserRoleModel(UserRoleModel model, UserRole userRole)
        {
            if (userRole != null)
            {
                //fill in model values from the entity
                model = new UserRoleModel
                {
                    Name = userRole.Name,
                    SystemName = userRole.SystemName,
                    Id = userRole.Id,
                    IsSystemRole = userRole.IsSystemRole,
                    Active = userRole.Active,
                    PermissionNames = _permissionService.GetPermissionRecordsByUserRoleId(userRole.Id).Select(s=>s.Name)
                };
            }

            //set default values for the new model
            if (userRole == null)
                model.Active = true;
            return model;
        }
        #endregion
    }
}