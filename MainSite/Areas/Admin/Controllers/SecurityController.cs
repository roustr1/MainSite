using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dal.Domain.Permissions;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Areas.Admin.Factories;
using MainSite.Areas.Admin.Models.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SecurityController : BaseAdminController
    {
        private readonly IUsersService _userService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<SecurityController> _logger;
        private readonly ISecurityModelFactory _securityModelFactory;

        #region ctor

        public SecurityController(IUsersService userService, IPermissionService permissionService, ILogger<SecurityController> logger, ISecurityModelFactory securityModelFactory)
        {
            _userService = userService;
            _permissionService = permissionService;
            _logger = logger;
            _securityModelFactory = securityModelFactory;
        }

        #endregion

        [Route("/Security/AccessDenied")]
        public virtual IActionResult AccessDenied(string pageUrl)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (user == null || !_userService.IsRegistered(user))
            {
                _logger.LogInformation($"Access denied to anonymous request on {pageUrl}");
                return View();
            }

            _logger.LogInformation($"Access denied to user #{user.SystemName} on {pageUrl}");

            return View();
        }

        [Route("admin/security/permissions")]
        [HttpGet, ActionName("Permissions")]
        public virtual IActionResult Permissions()
        {
#if RELEASE
             var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView();   
#endif
             
            //prepare model
            var model = _securityModelFactory.PreparePermissionMappingModel(new PermissionMappingModel());

            return View(model);
        }

        [Route("admin/security/permissions")]
        [HttpPost, ActionName("Permissions")]
        public virtual IActionResult PermissionsSave(PermissionMappingModel model, IFormCollection form)
        {
#if RELEASE
                 var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl, user))
                return AccessDeniedView(); 
#endif


            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var userRoles = _userService.GetAllUserRoles(true);

            foreach (var cr in userRoles)
            {
                var formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = !StringValues.IsNullOrEmpty(form[formKey])
                    ? form[formKey].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>();

                foreach (var pr in permissionRecords)
                {
                    var allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    //если права уже есть
                    if (allow == _permissionService.Authorize(pr.SystemName, cr.Id))
                        continue;

                    if (allow)
                    {
                        _permissionService.InsertPermissionRecordUserRoleMapping(new PermissionRecordUserRoleMapping { PermissionRecordId = pr.Id, UserRoleId = cr.Id });
                    }
                    else
                    {
                        _permissionService.DeletePermissionRecordUserRoleMapping(pr.Id, cr.Id);
                    }

                    _permissionService.UpdatePermissionRecord(pr);
                }
            }

            //   _notificationService.SuccessNotification( "Admin.Configuration.ACL.Updated");

            return RedirectToAction("Permissions");
        }


        [Route("Admin/InstallPermissions")]
        public IActionResult InstallPermissions()
        {
            _permissionService.InstallPermissions(new StandardPermissionProvider());
            return RedirectToAction("Index", "Users");
        }
    }
}
