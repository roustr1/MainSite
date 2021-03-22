using System.Collections.Generic;
using Application.Dal.Domain.Users;

namespace Application.Services.Users
{
    public interface IUsersService
    {
        /// <summary>
        /// Add a user-user role mapping
        /// </summary>
        /// <param name="roleMapping">User-user role mapping</param>
        void AddUserRoleMapping(UserUserRoleMapping roleMapping);

        /// <summary>
        /// Remove a user-user role mapping
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="role">User role</param>
        void RemoveUserRoleMapping(User user, UserRole role);

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void DeleteUserRole(UserRole userRole);

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        UserRole GetUserRoleById(string userRoleId);

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="systemName">User role system name</param>
        /// <returns>User role</returns>
        UserRole GetUserRoleBySystemName(string systemName);

        /// <summary>
        /// Get user role identifiers
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>User role identifiers</returns>
        string[] GetUserRoleIds(User user, bool showHidden = false);

        /// <summary>
        /// Gets list of user roles
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Result</returns>
        IList<UserRole> GetUserRoles(User user, bool showHidden = false);

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        IList<UserRole> GetAllUserRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void InsertUserRole(UserRole userRole);

        /// <summary>
        /// Gets a value indicating whether user is in a certain user role
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userRoleSystemName">User role system name</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsInUserRole(User user,
            string userRoleSystemName, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Gets a value indicating whether user is administrator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsAdmin(User user, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsForumModerator(User user, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        bool IsRegistered(User user, bool onlyActiveUserRoles = true);

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void UpdateUserRole(UserRole userRole);

        /// <summary>
        /// Get user Fio from current user 
        /// </summary>
        /// <returns></returns>
        string GetUserFio();

        /// <summary>
        /// return User's record depended by userName
        /// </summary>
        /// <param name="identiyName"></param>
        /// <returns></returns>
        User GetUserByIdentityName(string identiyName);
    }
}