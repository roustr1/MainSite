using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Permissions;
using Application.Dal.Domain.Users;
using Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        #region Fields
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<PermissionRecordUserRoleMapping> _permissionRecordUserRoleMappingRepository;
        private readonly IUsersService _userService;

        #endregion

        #region ctor

        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository, IRepository<PermissionRecordUserRoleMapping> permissionRecordUserRoleMappingRepository, IUsersService userService)
        {
            _permissionRecordRepository = permissionRecordRepository;
            _permissionRecordUserRoleMappingRepository = permissionRecordUserRoleMappingRepository;
            _userService = userService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get permission records by user role identifier
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>Permissions</returns>
        public virtual IList<PermissionRecord> GetPermissionRecordsByUserRoleId(string userRoleId)
        {

            var query = from pr in _permissionRecordRepository.GetAll()
                        join prcrm in _permissionRecordUserRoleMappingRepository.GetAll() on pr.Id equals prcrm
                            .PermissionRecordId
                        where prcrm.UserRoleId == userRoleId
                        orderby pr.Id
                        select pr;

            return query.ToList();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Delete(permission);

            //event notification
            //  _eventPublisher.EntityDeleted(permission);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordById(string permissionId)
        {
            if (permissionId == null)
                return null;

            return _permissionRecordRepository.Get(permissionId);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionRecordRepository.GetAll()
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permissionRecord = query.FirstOrDefault();
            return permissionRecord;
        }

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionRecordRepository.GetAll()
                        orderby pr.Name
                        select pr;
            var permissions = query.ToList();
            return permissions;
        }

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Add(permission);

            //event notification
            //   _eventPublisher.EntityInserted(permission);
        }

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission));

            _permissionRecordRepository.Update(permission);

            //event notification
            // _eventPublisher.EntityUpdated(permission);
        }

        /// <summary>
        /// Install permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
  
        public virtual void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();
            //default user role mappings
            var defaultPermissions = permissionProvider.GetDefaultPermissions().ToList();

            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 != null)
                    continue;

                //new permission (install it)
                permission1 = new PermissionRecord
                {
                    Name = permission.Name,
                    SystemName = permission.SystemName,
                    Category = permission.Category
                };

                //save new permission
                InsertPermissionRecord(permission1);

                foreach (var defaultPermission in defaultPermissions)
                {
                    var userRole = _userService.GetUserRoleBySystemName(defaultPermission.systemRoleName);
                    if (userRole == null)
                    {
                        //new role (save it)
                        userRole = new UserRole
                        {
                            Name = defaultPermission.systemRoleName,
                            Active = true,
                            SystemName = defaultPermission.systemRoleName
                        };
                        _userService.InsertUserRole(userRole);
                    }

                    var defaultMappingProvided = defaultPermission.permissions.Any(p => p.SystemName == permission1.SystemName);

                    if (!defaultMappingProvided)
                        continue;

                    InsertPermissionRecordUserRoleMapping(new PermissionRecordUserRoleMapping { UserRoleId = userRole.Id, PermissionRecordId = permission1.Id });
                }
            }
        }

        /// <summary>
        /// Uninstall permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        public virtual void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            var permissions = permissionProvider.GetPermissions();
            foreach (var permission in permissions)
            {
                var permission1 = GetPermissionRecordBySystemName(permission.SystemName);
                if (permission1 == null)
                    continue;

                DeletePermissionRecord(permission1);
            }
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return Authorize(permission.SystemName, user);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName, User user)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var userRoles = _userService.GetUserRoles(user);
            foreach (var role in userRoles)
                if (Authorize(permissionRecordSystemName, role.Id))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual bool Authorize(string permissionRecordSystemName, string userRoleId)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var permissions = GetPermissionRecordsByUserRoleId(userRoleId);
            foreach (var permission in permissions)
                if (permission.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;

        }

        /// <summary>
        /// Gets a permission record-user role mapping
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        public virtual IList<PermissionRecordUserRoleMapping> GetMappingByPermissionRecordId(string permissionId)
        {
            var query = _permissionRecordUserRoleMappingRepository.GetAll();

            query = query.Where(x => x.PermissionRecordId == permissionId);

            return query.ToList();
        }

        /// <summary>
        /// Delete a permission record-user role mapping
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <param name="userRoleId">User role identifier</param>
        public virtual void DeletePermissionRecordUserRoleMapping(string permissionId, string userRoleId)
        {
            var mapping = _permissionRecordUserRoleMappingRepository.GetAll().FirstOrDefault(prcm => prcm.UserRoleId == userRoleId && prcm.PermissionRecordId == permissionId);

            if (mapping is null)
                throw new Exception(string.Empty);

            _permissionRecordUserRoleMappingRepository.Delete(mapping);

            //event notification
            // _eventPublisher.EntityDeleted(mapping);
        }

        /// <summary>
        /// Inserts a permission record-user role mapping
        /// </summary>
        /// <param name="permissionRecordUserRoleMapping">Permission record-user role mapping</param>
        public virtual void InsertPermissionRecordUserRoleMapping(PermissionRecordUserRoleMapping permissionRecordUserRoleMapping)
        {
            if (permissionRecordUserRoleMapping is null)
                throw new ArgumentNullException(nameof(permissionRecordUserRoleMapping));

            _permissionRecordUserRoleMappingRepository.Add(permissionRecordUserRoleMapping);

            //event notification
            //    _eventPublisher.EntityInserted(permissionRecordUserRoleMapping);
        }

        #endregion


    }
}