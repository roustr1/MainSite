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

namespace MainSite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MainModel _mainMode;
        private readonly IPermissionService _permissionService;
        private readonly IUsersService _usersService;

        private static int _pagesize;

        public HomeController(MainModel mainMode, IPermissionService permissionService, IUsersService usersService)
        {
            _mainMode = mainMode;
            _permissionService = permissionService;
            _usersService = usersService;
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

        [Route("Create")]
        [HttpGet]
        public IActionResult Create(string currentCategory)
        {
            var user = _usersService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.EditNews, user))
                return AccessDeniedView();
            var model = new NewsItemViewModel(currentCategory);
            return View(model);
        }

        //[Route("Create")]
        [HttpPost("{Create}")]

        public IActionResult Create([FromForm] NewsItemViewModel model)
        {
            var user = _usersService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.EditNews, user))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                model.UploadedFiles = Request.Form.Files.ToList();
          
                model.Author = User.Identity.Name;
                _mainMode.CreateNewNewsItem(model);
            } 

            return JsonConvert.SerializeObject(_mainMode.GetNewsItemViewModel(model.Id));
           // return RedirectToAction(nameof(Index));
        }

        }
        /*CREATE_EXTEND*/
        [Route("CreateExtend")]
        [HttpGet]
        public IActionResult CreateExtend(string currentCategory = null)
        {
            var user = _usersService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.EditNews, user))
                return AccessDeniedView();

            var model = new NewsItemViewModel(currentCategory);
            return View("Create_Editor", model);
        }

        /*END CREATE_EXTEND*/
        [Route("Details")]
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction("Error");

            var model = _mainMode.GetNewsItemViewModel(id);
            if (model == null) return RedirectToAction("Error");

            return View(model);
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
        public IActionResult PinNews(string newsItemId, string currentCategoryId=null, int currentPage = 0)
        {
            _mainMode.PinNewsItem(newsItemId);
            return RedirectToAction("Index", new {page = currentPage, category = currentCategoryId});
        }

        [HttpPost]
        public IActionResult UnpinNews(string newsItemId, string currentCategoryId = null, int currentPage = 0)
        {
            _mainMode.UnpinNewsItem(newsItemId);
            return RedirectToAction("Index", new { page = currentPage, category = currentCategoryId });
        }
    }
}
