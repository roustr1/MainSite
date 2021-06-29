using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.Permissions;
using Application.Services.Permissions;
using Application.Services.Users;
using Application.Services.Utils;
using MainSite.Areas.Admin.Models.Security;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Factories
{

    /// <summary>
    /// Represents the security model factory implementation
    /// </summary>
    public partial class SecurityModelFactory : ISecurityModelFactory
    {
        #region Fields

        private readonly IUsersService _userService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public SecurityModelFactory(IUsersService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        #endregion

        #region VMMethods

        /// <summary>
        /// Prepare permission mapping model
        /// </summary>
        /// <param name="model">Permission mapping model</param>
        /// <returns>Permission mapping model</returns>
        public virtual PermissionMappingModel PreparePermissionMappingModel(PermissionMappingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var userRoles = _userService.GetAllUserRoles(true);
            model.AvailableUserRoles = userRoles.Select(role => new UserRoleModel
            {
                Id = role.Id,
                Name = role.Name,
                Active = role.Active,
                IsSystemRole = role.IsSystemRole,
                SystemName = role.SystemName
            }).ToList(); // role.ToModel<UserRoleModel>()).ToList();

            foreach (var permissionRecord in _permissionService.GetAllPermissionRecords())
            {
                model.AvailablePermissions.Add(new PermissionRecordModel
                {
                    Name = permissionRecord.Name,
                    SystemName = permissionRecord.SystemName
                });

                foreach (var role in userRoles)
                {
                    if (!model.Allowed.ContainsKey(permissionRecord.SystemName))
                        model.Allowed[permissionRecord.SystemName] = new Dictionary<string, bool>();
                    model.Allowed[permissionRecord.SystemName][role.Id] =
                        _permissionService.GetMappingByPermissionRecordId(permissionRecord.Id).Any(mapping => mapping.UserRoleId == role.Id);
                }
            }

            return model;
        }

        #endregion

        #region Factories
        /// <summary>
        /// Return new example of permission record
        /// </summary>
        /// <param name="name">Permission name</param>
        /// <param name="systemName">System name</param>
        /// <param name="category">Category</param>
        /// <returns></returns>

        public virtual PermissionRecord CreatePermissionRecord([NotNull] string name, string systemName, string category)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(systemName) || string.IsNullOrEmpty(category))
                throw new ArgumentNullException(nameof(PermissionRecord));
            return new PermissionRecord
            {
                Category = category,
                Name = name,
                SystemName = systemName
            };
        }

        public virtual PermissionRecord CreatePermissionRecordForMenu(MenuItem menuItem)
        {
            return CreatePermissionRecord(
                "Редактировать " + menuItem.Name,
                new TranslitMethods.Translitter().Translit(menuItem.Name, TranslitMethods.TranslitType.Gost)
                , nameof(MenuItem));
        }
        #endregion


    }

}
