using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using MainSite.ViewModels.UI.Menu;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: MenuService

        [Route("Admin/Menu")]
        public IActionResult Index()
        {
            var menuItems = _menuService.GetAll();
            var menuItemsViewModels = new List<MenuItemViewModel>();
            foreach (var item in menuItems)
            {
                if(item == null) continue;

                var vm = new MenuItemViewModel()
                {
                    Name = item.Name,
                    Id = item.Id
                };

                menuItemsViewModels.Add(vm);

            }
            return View(menuItemsViewModels);
        }


        [Route("Admin/Menu/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new List<SelectListItem> { new SelectListItem("Admin", "1"), new SelectListItem("moderator", "2") };
            ViewBag.MenuId = _menuService.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id }).ToList();
            return View();
        }

        // POST: MenuService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Menu/Create")]
        public IActionResult Create(MenuItem model)
        {
            if (ModelState.IsValid)
            {
                _menuService.InsertItem(model);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: MenuService/Delete/5
        [HttpGet]
        [Route("Admin/Menu/Delete")]
        public ActionResult Delete(string id)
        {
            var item = _menuService.Get(id);
            if(item!=null)
                _menuService.DeleteItem(item);
            return RedirectToAction("Index");
        }
    }
}
