using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Application.Dal.Domain.Users;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Areas.Admin.Factories;
using MainSite.Areas.Admin.Models.Users;
using MainSite.Filters;

namespace MainSite.Areas.Admin.Controllers
{

    public class UserRoleController : BaseAdminController
    {
        #region fields

        private readonly IPermissionService _permissionService;
        private readonly IUsersService _userService;
        private readonly IUserRoleModelFactory _userRoleModelFactory;
        #endregion
        #region CTOR

        public UserRoleController(IPermissionService permissionService, IUsersService userService, IUserRoleModelFactory userRoleModelFactory)
        {
            _permissionService = permissionService;
            _userService = userService;
            _userRoleModelFactory = userRoleModelFactory;
        }
        #endregion
        [Route("Admin/UserRoles/Index")]
        public virtual IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }
        [Route("Admin/UserRoles/List")]
        public virtual IActionResult List()
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user))
                return AccessDeniedView();

            //prepare model
            var roles = _userService.GetAllUserRoles(true);
            var model = roles.Select(role => _userRoleModelFactory.PrepareUserRoleModel(new UserRoleModel(), role));
            return View("List", model);
        }



        [Route("Admin/UserRoles/Create")]
        public virtual IActionResult Create()
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user)
                || !_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(new UserRoleModel(), null);

            return View(model);
        }
        [Route("Admin/UserRoles/Create")]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(UserRoleModel model, bool continueEditing)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user) || !_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var UserRole = new UserRole
                {
                    Name = model.Name,
                    Id = model.Id,
                    Active = true,
                    IsSystemRole = model.IsSystemRole,
                    SystemName = model.SystemName
                };
                _userService.InsertUserRole(UserRole);


                //_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = UserRole.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _userRoleModelFactory.PrepareUserRoleModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        [Route("Admin/UserRoles/Edit")]
        public virtual IActionResult Edit(string id)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user)
                || !_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var UserRole = _userService.GetUserRoleById(id);
            if (UserRole == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(null, UserRole);

            return View(model);
        }
        [Route("Admin/UserRoles/Edit")]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(UserRoleModel model, bool continueEditing)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user)
                || !_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var UserRole = _userService.GetUserRoleById(model.Id);
            if (UserRole == null)
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (UserRole.IsSystemRole && !model.Active)
                        throw new Exception("CantEditSystem");

                    if (UserRole.IsSystemRole && !UserRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new Exception("CantEditSystem");


                    //change all parameters available from the view
                    UserRole.Active = model.Active;
                    UserRole.Name = model.Name;
                    UserRole.SystemName = model.SystemName;

                    _userService.UpdateUserRole(UserRole);

                    //activity log
                    //_customerActivityService.InsertActivity("EditUserRole",
                    //    string.Format(_localizationService.GetResource("ActivityLog.EditUserRole"), UserRole.Name), UserRole);

                    //_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Updated"));

                    return continueEditing ? RedirectToAction("Edit", new { id = UserRole.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _userRoleModelFactory.PrepareUserRoleModel(model, UserRole);

                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                //   _notificationService.ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = UserRole.Id });
            }
        }

        [HttpPost]
        [Route("Admin/UserRoles/Delete")]
        public virtual IActionResult Delete(string id)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers, user)
                || !_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var UserRole = _userService.GetUserRoleById(id);
            if (UserRole == null)
                return RedirectToAction("List");

            try
            {
                _userService.DeleteUserRole(UserRole);


                //_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                // _notificationService.ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = UserRole.Id });
            }
        }
    }
}

