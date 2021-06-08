using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.ViewModels.UI.Menu;
using Microsoft.AspNetCore.Mvc.Rendering;
using MainSite.Models;
using Application.Services.Files;
using System;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : BaseAdminController
    {
        private readonly IMenuService _menuService;
        private readonly IUsersService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _uploadService;
        private readonly MainModel _mainMode;

        public MenuController(MainModel mainMode, IPictureService uploadService, IMenuService menuService, IUsersService userService, IPermissionService permissionService)
        {
            _menuService = menuService;
            _userService = userService;
            _permissionService = permissionService;
            _mainMode = mainMode;
            _uploadService = uploadService;
        }

        // GET: MenuService

        [Route("Admin/Menu")]
        public IActionResult Index()
        {
#if RELEASE
     var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
#endif



            var menuItems = _menuService.GetAll();
            var menuItemsViewModels = MenuTreeGenerate();

            return View(menuItemsViewModels);
        }


        [Route("Admin/Menu/Create")]
        [HttpGet]
        public IActionResult Create(string id)
        {
#if RELEASE
     var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
#endif      
            MenuItem model = null;
            if(!string.IsNullOrWhiteSpace(id))
            {
                model = _menuService.Get(id);              
            }

            //ViewBag.Roles = new List<SelectListItem> { new SelectListItem("Admin", "1"), new SelectListItem("moderator", "2") };
            ViewBag.MenuId = _menuService.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id }).ToList();
            return View(model);
        }

        // POST: MenuService/Create
        [HttpPost]
        [Route("Admin/Menu/Create")]
        public IActionResult Create([FromForm] MenuItem model)
        {
#if RELEASE
     var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
#endif
            if (ModelState.IsValid)
            {
                var fileForm = Request.Form.Files.Count() > 0 ? Request.Form.Files[0] : null;

                if (fileForm != null)
                {
                    var file = _uploadService.InsertPicture(fileForm);
                    _uploadService.GetPictureUrl(file.Id);

                    model.UrlIcone = _uploadService.GetPictureUrl(file.Id);
                }

                var entity = model.Id != null ? _menuService.Get(model.Id) : null;

                if (entity != null)
                {
                    entity.Name = model.Name;
                    entity.IsActive = model.IsActive;
                    entity.ParentId = model.ParentId;
                    entity.ToolTip = model.ToolTip;
                    if (!String.IsNullOrWhiteSpace(model.UrlIcone)) entity.UrlIcone = model.UrlIcone;

                    _menuService.UpdateItem(entity);
                }
                else
                {
                    _menuService.InsertItem(model);
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: MenuService/Delete/5
        [HttpGet]
        [Route("Admin/Menu/Delete")]
        public ActionResult Delete(string id)
        {
            #if RELEASE
                 var user = _userService.GetUserBySystemName(User.Identity.Name);
                        if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                            return AccessDeniedView();
            #endif
            var item = _menuService.Get(id);
            if (item != null)
            {
                _menuService.DeleteItem(item);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin/Menu/ItemUp")]
        public IActionResult ItemUp(string id)
        {
#if RELEASE
     var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
#endif
            ItemMove(id: id, toDown: false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin/Menu/ItemDown")]
        public IActionResult ItemDown(string id)
        {
#if RELEASE
     var user = _userService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMenu, user))
                return AccessDeniedView();
#endif
            ItemMove(id: id, toDown: true);

            return RedirectToAction("Index");
        }

        [NonAction]
        private void ItemMove(string id, bool toDown)
        {
            var item = _menuService.Get(id);
            var category = item.ParentId;
            var index = item.Index;

            if (item != null)
            {
                var newIndex = NewIndex(category, index, toDown: toDown);
                if (item.Index != newIndex)
                {
                    var item2 = GetItemByCategoryAndIndex(category, newIndex);

                    if (item2 != null)
                    {
                        item2.Index = index;
                        _menuService.UpdateItem(item2);
                    }

                    item.Index = newIndex;
                    _menuService.UpdateItem(item);
                }
            }
        }
        [NonAction]
        private IEnumerable<MenuItemViewModel> MenuTreeGenerate(string categoryId = null)
        {
            var result = new List<MenuItemViewModel>();

            CreateTree(null, result);

            return result;
        }
        [NonAction]
        private void CreateTree(string id, List<MenuItemViewModel> list)
        {
            foreach (var item in _menuService.GetManyByParentId(id).OrderBy(i => i.Index))
            {
                var itemViewModel = new MenuItemViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ToolTip = item.ToolTip,
                    UrlIcon = item.UrlIcone,
                    IsActive = true,
                    Index = item.Index

                };

                list.Add(itemViewModel);
            }

            foreach (var item in list)
            {
                CreateTree(item.Id, item.Children);
            }
        }
        [NonAction]
        private int NewIndex(string categoryId, int currentIndex, bool toDown)
        {
            var newIndex = currentIndex;
            var items = _menuService.GetManyByParentId(categoryId).ToList();
            if (items.Any())
            {
                var item = items.OrderBy(m => m.Index).LastOrDefault();
                var maxIndex = item != null ? item.Index : 0;

                if (maxIndex > 0)
                {
                    if (currentIndex > 0 && currentIndex < maxIndex)
                    {
                        newIndex = toDown ? currentIndex + 1 : currentIndex - 1;
                    }
                    else if (currentIndex == 0 && toDown)
                    {
                        newIndex = currentIndex + 1;
                    }
                    else if (currentIndex == maxIndex && !toDown)
                    {
                        newIndex = currentIndex - 1;
                    }
                }
            }

            return newIndex;
        }
        [NonAction]
        private MenuItem GetItemByCategoryAndIndex(string categoryId, int currentIndex)
        {
            return _menuService.GetManyByParentId(categoryId).FirstOrDefault(i => i.Index == currentIndex);
        }
    }
}
