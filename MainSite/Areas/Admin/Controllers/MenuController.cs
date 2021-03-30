using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Areas.Admin.Factories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseAdminController
    {
        private readonly IMenuService _menuService;
        private readonly IUsersService _userService;
        private readonly IPermissionService _permissionService;


        public MenuController(IMenuService menuService, IUsersService userService, IPermissionService permissionService)
        {
            _menuService = menuService;
            _userService = userService;
            _permissionService = permissionService;
        }

        // GET: MenuService

        [Route("Admin/Index")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("Admin/CreateMenu")]
        [HttpGet]
        public IActionResult Create()
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
            ViewBag.Roles = new List<SelectListItem> { new SelectListItem("Admin", "1"), new SelectListItem("moderator", "2") };
            ViewBag.MenuId = _menuService.GetMenuItem().Select(s => new SelectListItem { Text = s.Name, Value = s.Id }).ToList();
            return View();
        }

        // POST: MenuService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/CreateMenu")]
        public IActionResult Create(MenuItem model)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {
                _menuService.InsertItem(model);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
 

        // POST: MenuService/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
            var item = _menuService.GetItem(id);
            if(item!=null)
                _menuService.DeleteItem(item);
            return RedirectToAction("Index");

        }
    }
}
