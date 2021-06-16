using MainSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MainSite.Models;
using MainSite.ViewModels.News;
using Newtonsoft.Json;
using System.Linq;
using Application.Dal.Domain.Menu;
using Application.Dal.Infrastructure;
using Application.Services.Menu;
using Application.Services.Files;
using MainSite.ViewModels.UI.Menu;

namespace MainSite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MainModel _mainMode;


        private readonly IMenuService _menuService;
        private readonly IPictureService _uploadService;
        private readonly IFileDownloadService _downloadService;
        private readonly IAppFileProvider _fileProvider;

        public HomeController(MainModel mainMode, IMenuService menuService, IPictureService uploadService, IFileDownloadService downloadService, IAppFileProvider fileProvider)
        {
            _mainMode = mainMode;
            _menuService = menuService;
            _uploadService = uploadService;
            _downloadService = downloadService;
            _fileProvider = fileProvider;
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
                if (model.IsAdvancedEditor)
                {
                    _mainMode.InsertAdvancedNewsItem(model);
                }

                //model.Author = User.Identity.Name;
                _mainMode.CreateNewNewsItem(model, User);
            }

            return Json(JsonConvert.SerializeObject(_mainMode.GetNewsItemViewModel(model.Id)));
            // return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Edit([FromForm] NewsItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UploadedFiles = Request.Form.Files.ToList();
                if (model.IsAdvancedEditor)
                {
                    _mainMode.InsertAdvancedNewsItem(model);
                }
           
                _mainMode.EditNewNewsItem(model, User);
                return Json(JsonConvert.SerializeObject(_mainMode.GetNewsItemViewModel(model.Id)));
            }

            return Json(null);
        }

        [HttpGet]
        [Route("GetFile")]
        public IActionResult DownloadFile(string fileId)
        {
            return _mainMode.GetDownloadFile(fileId);
        }


        // POST: MenuService/Create
        [HttpPost]
        public IActionResult CreateCategory(MenuItem model)
        {
            if (ModelState.IsValid)
            {
                var fileForm = Request.Form.Files.Any() ? Request.Form.Files[0] : null;
                if (fileForm != null)
                {

                    var file = _uploadService.InsertPicture(fileForm);
                    _uploadService.GetPictureUrl(file.Id);

                    model.UrlIcone = _uploadService.GetPictureUrl(file.Id);
                }

                _menuService.InsertItem(model);

                var itemViewModel = new MenuItemViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    ToolTip = model.ToolTip,
                    UrlIcon = model.UrlIcone,
                    ParentId = model.ParentId,
                    Index = model.Index

                };

                return Json(JsonConvert.SerializeObject(itemViewModel));
            }
            return Json(null);
        }

        /// <summary>
        /// Delete news item
        /// </summary>
        /// <param name="id">News item identifier</param>
        /// <returns></returns>
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