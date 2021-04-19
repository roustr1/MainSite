using MainSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Application.Services.Permissions;
using Application.Services.Users;
using MainSite.Models;
using MainSite.ViewModels.News;

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
            var user = _usersService.GetUserBySystemName(User.Identity.Name);
            if (!_permissionService.Authorize(StandardPermissionProvider.AccessToIndexPage, user))
                return AccessDeniedView();

            var model = _mainMode.GetNewsListViewModel(page, _pagesize, category);
            return View(model);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create(string currentCategory)
        {
            var model = new NewsItemViewModel();
            return View(model);
        }

        //[Route("Create")]
        [HttpPost("{Create}")]
        public IActionResult Create([FromForm] NewsItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mainMode.CreateNewNewsItem(model);
            }
            return RedirectToAction(nameof(Index));
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
         //   var file = _mainMode.GetDownloadedFileViewModel(fileId);
         //   if (file == null) return new EmptyResult();

            var file = _mainMode.GeDownloadedFile(fileId);
            string fileName = $"{file.OriginalName}{file.LastPart}";
            return File(file.FileBinary, file.MimeType, fileName);
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

        public IActionResult BirtdayView()
        {
            //  var url = _mainMode.
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
