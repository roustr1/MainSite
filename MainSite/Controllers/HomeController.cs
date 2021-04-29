using MainSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Models;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using Application.Services.Files;

namespace MainSite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MainModel _mainMode;
        private readonly IPermissionService _permissionService;
        private readonly IUsersService _usersService;
 
        private readonly IMenuService _menuService;
        private readonly IFileUploadService _uploadService;

        private static int _pagesize;

        public HomeController(MainModel mainMode, IPermissionService permissionService, IUsersService usersService, IMenuService menuService, IFileUploadService fileUploadService)
        {
            _mainMode = mainMode;
            _permissionService = permissionService;
            _usersService = usersService;
            _menuService = menuService;
            _uploadService = fileUploadService;
            SetPageSize();
        }
        private void SetPageSize()
        {
            _pagesize = _mainMode.GetSettingNewsPerPage();
        }
        public IActionResult Index(int page = 0, string category = null)
        {
            return View();
        }
 
        [HttpPost]
        public IActionResult Create([FromForm] NewsItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UploadedFiles = Request.Form.Files.ToList();

                model.Author = User.Identity.Name;
                _mainMode.CreateNewNewsItem(model);
 
            } 

            return Json(JsonConvert.SerializeObject(_mainMode.GetNewsItemViewModel(model.Id)));
           // return RedirectToAction(nameof(Index));
        }

 
        [HttpGet]
        [Route("GetFile")]
        public IActionResult GetFile(string fileId)
        {
            var file = _mainMode.GetDownloadedFileViewModel(fileId);
            if (file == null) return new EmptyResult();
            var fileBinary = _mainMode.GeDownloadedFile(fileId);
            return File(fileBinary, file.MimeType, file.Name);
        }
 

        // POST: MenuService/Create
        [HttpPost]
        public IActionResult CreateCategory(MenuItem model)
        {
            if (ModelState.IsValid)
            {
                var fileForm = Request.Form.Files.Count() > 0 ? Request.Form.Files[0] : null;
                if (fileForm != null)
                {
                    _uploadService.StoreInDb = true;
                    var insertFile = _uploadService.InsertFile(fileForm);
                    model.UrlIcone = insertFile.StoredName;
                }

                _menuService.InsertItem(model);
                return Json(JsonConvert.SerializeObject(model));
            }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return Error();
            var item = _mainMode.GetNewsItemViewModel(id);
            if (item == null) return Error();
            _mainMode.DeleteNewsItem(id);
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult PinNews(string newsItemId, string currentCategoryId = null, int currentPage = 0)
        {
            _mainMode.PinNewsItem(newsItemId);
            return RedirectToAction("Index", new { page = currentPage, category = currentCategoryId });
        }
        [HttpPost]
        public IActionResult UnpinNews(string newsItemId, string currentCategoryId = null, int currentPage = 0)
        {
            _mainMode.UnpinNewsItem(newsItemId);
            return RedirectToAction("Index", new { page = currentPage, category = currentCategoryId });
        }
    }
}