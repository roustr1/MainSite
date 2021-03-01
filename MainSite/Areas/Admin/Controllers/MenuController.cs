using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
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
            if (ModelState.IsValid)
            {
                _menuService.InsertItem(model);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }



        // GET: MenuService/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuService/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
