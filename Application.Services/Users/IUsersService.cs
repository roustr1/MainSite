using System;
using System.Collections.Generic;
using System.Security.Claims;
using Application.Dal;
using Application.Dal.Domain.Users;

namespace Application.Services.Users
{
    public interface IUsersService
    {

        #region UserRoles 
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
        /// Получить список ролей конкретного пользователя
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
        #endregion

        #region Users

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="username">Username; null to load all customers</param> 
        /// <param name="ipAddress">IP address; null to load all customers</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Users</returns>
        IPagedList<User> GetAllUsers(  string[] customerRoleIds = null,
             string username = null, string ipAddress = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

 
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">User</param>
        void DeleteUser(User customer);

 

 
        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">User identifier</param>
        /// <returns>A customer</returns>
        User GetUserById(string customerId);

        /// <summary>
        /// Get customers by identifiers
        /// </summary>
        /// <param name="customerIds">User identifiers</param>
        /// <returns>Users</returns>
        IList<User> GetUsersByIds(string[] customerIds);

        /// <summary>
        /// Get all customers from one subdivision
        /// </summary>
        /// <param name="subidivision"></param>
        /// <returns></returns>
        IList<User> GetUsersBySubDivision(string subidivision);
 

        /// <summary>
        /// Get customer by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        User GetUserBySystemName(ClaimsPrincipal principal);

        ///// <summary>
        ///// Get customer by username
        ///// </summary>
        ///// <param name="username">Username</param>
        ///// <returns>User</returns>
        //User GetUserByUsername(string username);

 

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">User</param>
        void InsertUser(User customer);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">User</param>
        void UpdateUser(User customer);
 
 
 
        /// <summary>
        /// Get user Fio from current user 
        /// </summary>
        /// <returns></returns>
        string GetUserNameFromAD(string userName);

        ///// <summary>
        ///// Add selected roles to user
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="roleIds"></param>
        //void AddRoleToUser(string userId, string[] roleIds);

        ///// <summary>
        ///// Add selected roles to user
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="roleIds"></param>
        //void AddRoleToUser(string userId, string roleId);
    }

    #endregion


}