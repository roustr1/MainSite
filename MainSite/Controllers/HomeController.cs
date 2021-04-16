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
            var model = new NewsItemViewModel();
            return View(model);
        }

        //[Route("Create")]
        [HttpPost]
        public string Create([FromForm] NewsItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UploadedFiles = Request.Form.Files.ToList();
                _mainMode.CreateNewNewsItem(model);
            } 

            return JsonConvert.SerializeObject(_mainMode.GetNewsItemViewModel(model.Id));
           // return RedirectToAction(nameof(Index));
        }

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
    }
}
