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
        /// <param name="model">Customer role model</param>
        /// <param name="UserRole">Customer role</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Customer role model</returns>
        public virtual UserRoleModel PrepareUserRoleModel(UserRoleModel model, UserRole UserRole)
        {
            if (UserRole != null)
            {
                //fill in model values from the entity
                model = new UserRoleModel
                {
                    Name = UserRole.Name,
                    SystemName = UserRole.SystemName,
                    Id = UserRole.Id,
                    IsSystemRole = UserRole.IsSystemRole,
                    Active = UserRole.Active,
                    PermissionNames = _permissionService.GetPermissionRecordsByUserRoleId(UserRole.Id).Select(s=>s.Name)
                };
            }

            //set default values for the new model
            if (UserRole == null)
                model.Active = true;
            return model;
        }
        #endregion
    }
}