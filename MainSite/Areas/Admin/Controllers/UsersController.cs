using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Permissions;
using Application.Dal.Domain.Users;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Areas.Admin.Factories;
using MainSite.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Primitives;
using UserModelFactory = MainSite.Areas.Admin.Factories.UserModelFactory;

namespace MainSite.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {

        private readonly IUserModelFactory _userModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IUsersService _userService;

        #region ctor

        public UsersController(IUserModelFactory userModelFactory, IPermissionService permissionService, IUsersService userService)
        {
            _userModelFactory = userModelFactory;
            _permissionService = permissionService;
            _userService = userService;
        }


        #endregion

        #region Users
        [Route("Admin/Users/Index")]
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }
        [Route("Admin/Users/List")]
        public virtual IActionResult List()
        {
#if RELEASE
              var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user))
                return AccessDeniedView();   
#endif


            //prepare model
            var model = _userModelFactory.PrepareUserModelList(new UserSearchModel());
            ViewBag.RolesList = _userService.GetAllUserRoles(true).Select(r => new SelectListItem
            {
                Text = $"{r.Name}-{r.SystemName}",
                Value = r.Id
            });
            return View(model);
        }

        //[HttpPost]
        //public virtual IActionResult UserList(UserSearchModel searchModel)
        //{
        //    var user = _userService.GetUserBySystemName(User.Identity.Name);
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user))
        //        return AccessDeniedDataTablesJson();

        //    //prepare model
        //    var model = _userModelFactory.PrepareUserModelList(searchModel);

        //    return Json(model);
        //}
        #endregion

        #region UserRoles
        [HttpGet]
        [Route("Admin/Users/AddRoleToUser")]
        public IActionResult AddRolesToUser(string id)
        {
#if RELEASE
              var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user))
                return AccessDeniedView();   
#endif
            if (string.IsNullOrEmpty(id)) return ErrorJson("Пользователь не указан");



            //модель пользвателя с его текущими ролями
            var model = _userModelFactory.PrepareAddUserRoleModel(new UserAddUserRoleModel(id));
            return View(model);
        }


        [HttpPost]
        [Route("Admin/Users/AddRoleToUser")]
        public IActionResult AddRolesToUser(string id, IFormCollection form)
        {

#if RELEASE
              var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user))
                return AccessDeniedView();   
#endif

            //if (string.IsNullOrEmpty(id) || roleIds == null)

            //    return RedirectToAction("Index");
            //var user = _userService.GetUserById(id);

            //_userService.AddRoleToUser(id, roleIds);
            var userRoles = _userService.GetAllUserRoles(true);
            var user = _userService.GetUserById(id);

            var formKey = "allow_" + id;// ur.Id;
            var userRolesSystemNamesToRestrict = !StringValues.IsNullOrEmpty(form[formKey])
                ? form[formKey].ToString()?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>();

            foreach (var ur in userRoles)
            {
                var allow = userRolesSystemNamesToRestrict.Contains(ur.SystemName);
                //проверить, есть ли данная роль у пользователя в данный момент
                if (allow == _userService.GetUserRoles(user).Contains(ur))
                    continue;

                if (allow)
                {
                    _userService.AddUserRoleMapping(new UserUserRoleMapping
                    {
                        UserId = id,
                        UserRoleId = ur.Id
                    });
                }
                else
                {
                    _userService.RemoveUserRoleMapping(user, ur);
                }
                //_permissionService.UpdatePermissionRecord(ur);
                _userService.UpdateUserRole(ur);
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}
