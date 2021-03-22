using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Users;

namespace Application.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<UserUserRoleMapping> _userUserRoleMappingRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UsersService(IRepository<UserUserRoleMapping> userUserRoleMappingRepository, IRepository<UserRole> userRoleRepository)
        {
            _userUserRoleMappingRepository = userUserRoleMappingRepository;
            _userRoleRepository = userRoleRepository;
        }
        #region User roles

        /// <summary>
        /// Add a user-user role mapping
        /// </summary>
        /// <param name="roleMapping">User-user role mapping</param>
        public void AddUserRoleMapping(UserUserRoleMapping roleMapping)
        {
            if (roleMapping is null)
                throw new ArgumentNullException(nameof(roleMapping));

            _userUserRoleMappingRepository.Add(roleMapping);

           // _eventPublisher.EntityInserted(roleMapping);
        }

        /// <summary>
        /// Remove a user-user role mapping
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="role">User role</param>
        public void RemoveUserRoleMapping(string userName, UserRole role)
        {
            if (userName is null)
                throw new ArgumentNullException(nameof(userName));

            if (role is null)
                throw new ArgumentNullException(nameof(role));

            var mapping = _userUserRoleMappingRepository.GetAll().SingleOrDefault(ccrm => ccrm.UserName == userName && ccrm.UserRoleId == role.Id);

            if (mapping != null)
            {
                _userUserRoleMappingRepository.Delete(mapping);

                //event notification
              //  _eventPublisher.EntityDeleted(mapping);
            }
        }

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            if (userRole.IsSystemRole)
                throw new Exception("System role could not be deleted");

            _userRoleRepository.Delete(userRole);

            //event notification
           // _eventPublisher.EntityDeleted(userRole);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleById(string userRoleId)
        {
            if (string.IsNullOrEmpty(userRoleId))
                return null;

            return _userRoleRepository.Get(userRoleId);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="systemName">User role system name</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;


            var query = from cr in _userRoleRepository.GetAll()
                        orderby cr.Id
                        where cr.SystemName == systemName
                        select cr;
            var userRole = query.FirstOrDefault();

            return userRole;
        }

        /// <summary>
        /// Get user role identifiers
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>User role identifiers</returns>
        public virtual string[] GetUserRoleIds(string userName, bool showHidden = false)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            var query = from cr in _userRoleRepository.GetAll()
                        join crm in _userUserRoleMappingRepository.GetAll() on cr.Id equals crm.UserRoleId
                        where crm.UserName == userName &&
                        (showHidden || cr.Active)
                        select cr.Id;


            return query.ToArray();
        }

        /// <summary>
        /// Gets list of user roles
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Result</returns>
        public virtual IList<UserRole> GetUserRoles(string userName, bool showHidden = false)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            var query = from ur in _userRoleRepository.GetAll()
                        join urm in _userUserRoleMappingRepository.GetAll() on ur.Id equals urm.UserRoleId
                        where urm.UserName == userName &&
                              (showHidden || ur.Active)
                        select ur;

            return query.ToList();
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        public virtual IList<UserRole> GetAllUserRoles(bool showHidden = false)
        {
            var query = from cr in _userRoleRepository.GetAll()
                        orderby cr.Name
                        where showHidden || cr.Active
                        select cr;

            var userRoles = query.ToList();

            return userRoles;
        }

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Add(userRole);

            ////event notification
            //_eventPublisher.EntityInserted(userRole);
        }

        /// <summary>
        /// Gets a value indicating whether user is in a certain user role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleSystemName">User role system name</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsInUserRole(string user,
            string userRoleSystemName, bool onlyActiveUserRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(userRoleSystemName))
                throw new ArgumentNullException(nameof(userRoleSystemName));

            var userRoles = GetUserRoles(user, !onlyActiveUserRoles);

            return userRoles?.Any(cr => cr.SystemName == userRoleSystemName) ?? false;
        }

        /// <summary>
        /// Gets a value indicating whether user is administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsAdmin(string user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.AdministratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsForumModerator(string user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.ForumModeratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(string user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.RegisteredRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Update(userRole);

            ////event notification
            //_eventPublisher.EntityUpdated(userRole);
        }

        #endregion

    }
}