using System;
using System.Collections.Generic;
using System.Data.Common;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Users;

namespace Application.Services.Users
{
    public class UsersService : IUsersService
    {
        #region Fields

        private readonly IRepository<UserUserRoleMapping> _userUserRoleMappingRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<User> _userRepository;

        #endregion

        #region Ctor

        public UsersService(IRepository<UserUserRoleMapping> userUserRoleMappingRepository,
            IRepository<UserRole> userRoleRepository, IRepository<User> userRepository)
        {
            _userUserRoleMappingRepository = userUserRoleMappingRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        #endregion

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

        //public void AddRoleToUser(string userId, string[] roleIds)
        //{
        //    if (string.IsNullOrEmpty(userId) || roleIds == null) return;
        //    var user = _userRepository.Get(userId);

        //    foreach (var roleId in roleIds)
        //    {
        //        var role = _userRoleRepository.Get(roleId);
        //        try
        //        {
        //            var userRoleMapping = new UserUserRoleMapping
        //            {
        //                UserId = user.Id,
        //                UserRoleId = role.Id
        //            };
        //            AddUserRoleMapping(userRoleMapping);
        //        }
        //        catch (DbException e)
        //        {
        //            e.Data.Add(user.SystemName, $"Не удалось добавить роль {AppUserDefaults.RegisteredRoleName}");
        //            throw;
        //        }
        //    }
        //}

        //public void AddRoleToUser(string userId, string roleId)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId)) return; 
        //    var user = _userRepository.Get(userId);
        //}

        /// <summary>
        /// Remove a user-user role mapping
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="role">User role</param>
        public void RemoveUserRoleMapping(User user, UserRole role)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (role is null)
                throw new ArgumentNullException(nameof(role));
            
            var mapping = _userUserRoleMappingRepository.GetAll()
                .SingleOrDefault(ccrm => ccrm.UserId == user.Id && ccrm.UserRoleId == role.Id);

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
        public virtual string[] GetUserRoleIds(User user, bool showHidden = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = from cr in _userRoleRepository.GetAll()
                        join crm in _userUserRoleMappingRepository.GetAll() on cr.Id equals crm.UserRoleId
                        where crm.UserId == user.Name &&
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
        public virtual IList<UserRole> GetUserRoles(User user, bool showHidden = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = from ur in _userRoleRepository.GetAll()
                        join urm in _userUserRoleMappingRepository.GetAll() on ur.Id equals urm.UserRoleId
                        where urm.UserId == user.Id &&
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
        public virtual bool IsInUserRole(User user,
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
        public virtual bool IsAdmin(User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.AdministratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is a forum moderator
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsForumModerator(User user, bool onlyActiveUserRoles = true)
        {
            return IsInUserRole(user, AppUserDefaults.ModeratorsRoleName, onlyActiveUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether user is registered
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="onlyActiveUserRoles">A value indicating whether we should look only in active user roles</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(User user, bool onlyActiveUserRoles = true)
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

        #region User



        public virtual string GetUserNameFromAD()
        {
            return UserPrincipal.Current.DisplayName;
        }

        private User CreateUser(string identityName)
        {
            var user = new User
            {
                SystemName = identityName,
                Active = true,
                Name = GetUserNameFromAD(),
                LastActivityDate = DateTime.Now,
                FullName = GetUserNameFromAD()
            };
            _userRepository.Add(user);
            var role = GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
            try
            {
                var userRoleMapping = new UserUserRoleMapping
                {
                    UserId = user.Id,
                    UserRoleId = role.Id
                };
                AddUserRoleMapping(userRoleMapping);
            }
            catch (DbException e)
            {
                //В случае ошибки при добавлении роли пользователю он будет удален из бд
                DeleteUser(user);
                e.Data.Add(user.SystemName, $"Не удалось добавить роль {AppUserDefaults.RegisteredRoleName}");
                throw;
            }

            return user;
        }

        public IPagedList<User> GetAllUsers(string[] customerRoleIds = null,
            string username = null, string ipAddress = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _userRepository.GetAll();
            query = query.Where(c => !c.Deleted);

            if (customerRoleIds != null && customerRoleIds.Length > 0)
            {
                query = query.Join(_userUserRoleMappingRepository.GetAll(), x => x.Id, y => y.UserId,
                        (x, y) => new { Customer = x, Mapping = y })
                    .Where(z => customerRoleIds.Contains(z.Mapping.UserRoleId))
                    .Select(z => z.Customer)
                    .Distinct();
            }


            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Name.Contains(username));



            //search by IpAddress
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                query = query.Where(w => w.LastIpAddress == ipAddress);
            }

            query = query.OrderByDescending(c => c.Name);

            var customers = new PagedList<User>(query.AsQueryable(), pageIndex, pageSize, getOnlyTotalCount);

            return customers;
        }

        public void DeleteUser(User customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Deleted = true;

            UpdateUser(customer);

            //event notification
            //   _eventPublisher.EntityDeleted(customer);
        }

        public User GetUserById(string customerId) => string.IsNullOrEmpty(customerId) ? null : _userRepository.Get(customerId);

        public IList<User> GetUsersByIds(string[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.GetAll()
                        where userIds.Contains(c.Id) && !c.Deleted
                        select c;
            var customers = query.ToList();
            //sort by passed identifiers

            return userIds.Select(id => customers.Find(x => x.Id == id)).Where(customer => customer != null).ToList();
        }


        public User GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _userRepository.GetAll()
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var customer = query.FirstOrDefault();
            if (customer != null) return customer;
            return CreateUser(systemName);
        }

        public void InsertUser(User customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _userRepository.Add(customer);

            //event notification
            //_eventPublisher.EntityInserted(customer);
        }

        public void UpdateUser(User customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _userRepository.Update(customer);

            //event notification
            //_eventPublisher.EntityUpdated(customer);
        }



        #endregion
    }
}