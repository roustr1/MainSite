using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Users;
using Application.Services.Users;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly IUsersService _userService;
        private readonly IUserRoleModelFactory _userRoleModelFactory;

        public UserModelFactory(IUsersService userService, IUserRoleModelFactory userRoleModelFactory)
        {
            _userService = userService;
            _userRoleModelFactory = userRoleModelFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SelectedRoleIds"></param>
        /// <param name="userName"></param>
        /// <param name="ipAddress"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public virtual IEnumerable<UserModel> PrepareUserModelList(UserSearchModel searchModel)
        {
            //get customers
            var customers = _userService.GetAllUsers(customerRoleIds: searchModel.SelectedRoleIds,
                username: searchModel.UserName,
                ipAddress: searchModel.IpAddress,
                pageIndex: searchModel.PageNum - 1, pageSize: searchModel.Pagesize);


            return customers.Select(c => PrepareModel(c));

        }

        private UserModel PrepareModel(User user)
        {
            var model = new UserModel
            {
                Id = user.Id,
                FullName = user.FullName,
                SystemName = user.SystemName,
                UserName = user.Name,
                IPAddress = user.LastIpAddress,
                UserRoles = _userService.GetUserRoles(user)
                    .Select(s => _userRoleModelFactory.PrepareUserRoleModel(null, s).Name)


            };

            return model;
        }


        public UserAddUserRoleModel PrepareAddUserRoleModel(UserAddUserRoleModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var userRoles = _userService.GetAllUserRoles(true);

            //проинициализировать предустановленными значениями
            foreach (var role in userRoles)
            {
                model.AvailableUserRoles.Add(new UserRoleModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Active = role.Active,
                    IsSystemRole = role.IsSystemRole,
                    SystemName = role.SystemName
                });
                if (!model.Allowed.ContainsKey(role.SystemName))
                    model.Allowed.Add(role.SystemName, false);
                model.Allowed[role.SystemName] =
                    _userService.GetUserRoles(
                        _userService.GetUserById(model.UserId)).Any(c => c.SystemName == role.SystemName);
            }

            //foreach (var permissionRecord in _permissionService.GetAllPermissionRecords())
            //{
            //    model.AvailablePermissions.Add(new PermissionRecordModel
            //    {
            //        Name = permissionRecord.Name,
            //        SystemName = permissionRecord.SystemName
            //    });

            //    foreach (var role in userRoles)
            //    {
            //        if (!model.Allowed.ContainsKey(permissionRecord.SystemName))
            //            model.Allowed[permissionRecord.SystemName] = new Dictionary<string, bool>();
            //        model.Allowed[permissionRecord.SystemName][role.Id] =
            //            _permissionService.GetMappingByPermissionRecordId(permissionRecord.Id).Any(mapping => mapping.UserRoleId == role.Id);
            //    }
            //}

            return model;

        }
    }
}