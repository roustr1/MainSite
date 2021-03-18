using System;
using System.Collections.Generic;
using MainSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Files;
using Application.Dal.Domain.News;
using Application.Services.Files;
using Application.Services.News;
using Application.Services.Settings;
using MainSite.ViewModels.Common;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MainSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IFileDownloadService _downloadService;
        private readonly IFileUploadService _uploadService;
        private readonly ISettingsService _settingsService;
        private static int _pagesize;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, IFileDownloadService downloadService, IFileUploadService uploadService, ISettingsService settingsService)
        {
            _logger = logger;
            _newsService = newsService;
            _downloadService = downloadService;
            _uploadService = uploadService;
            _settingsService = settingsService;
            SetPageSize();
        }

        private void SetPageSize()
        {
            _pagesize = 3;
            if (_settingsService.SettingsDictionary.TryGetValue("Page.PageSize", out var _value))
                int.TryParse(_value, out _pagesize);
        }


        public IActionResult Index(int page = 0, string category = null)
        {

            var model = _newsPaged(page, _pagesize, category);
            return View(model);
        }

        public IActionResult News(string category = null)
        {
            var news = _newsService.GetNewsItem(category: category);
            return View(news);
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
                var dataTimeNow = DateTime.Now;
                var entity = new NewsItem
                {
                    Header = model.Header,
                    Description = model.Description,
                    AutorFio = User?.Identity?.Name ?? "Неавторизован",
                    CreatedDate = dataTimeNow,
                    LastChangeDate = dataTimeNow,
                    Category =  model.Category,
                };
                var collection = new List<File>();
                //uploadFiles 
                foreach (var file in model?.UploadedFiles)
                {
                    collection.Add(_uploadService.InsertFile(file));
                }

                entity.Files = collection;
                _newsService.CreateNews(entity);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("Details")]
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (id == null) return RedirectToAction("Error");
            var model = _newsService.GetNewsItem(id);
            if (model == null) return RedirectToAction("Error");
            model.Files = _uploadService.GetFilesByNewsId(id).ToList();
            return View(model);
        }

        [HttpGet]
        [Route("GetFile")]
        public IActionResult  GetFile(string fileId)
        {
            if (fileId == null) return new EmptyResult();
            var file = _uploadService.GetFileById(fileId);
            var fileBinary = _uploadService.LoadFileBinary(file);
            var filename = $"{file.OriginalName}{file.LastPart}";
            return File(fileBinary, file.MimeType, filename);
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return Error();
            var item = _newsService.GetNewsItem(id);
            if (item == null) return Error();
            _newsService.DeleteNews(item);
            foreach (var file in item.Files)
            {
                _downloadService.DeleteDownload(file);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private NewsListViewModel _newsPaged(int? page, int? pagesize, string category = null)
        {

            int pageIndex = 0;
            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }
            var pageSize = pagesize.GetValueOrDefault(10);

            var records = new List<NewsItemViewModel>();
            foreach (var newsItem in _newsService.GetNewsItem(category: category))
            {
                if (newsItem == null) continue;

                var newsItemViewModel = new NewsItemViewModel()
                {
                    Id = newsItem.Id,
                    Header = newsItem.Header,
                    Description = newsItem.Description,
                    Category = newsItem.Category,
                    Author = newsItem.AutorFio,
                    CreatedDate = newsItem.CreatedDate,
                    LastChangeDate = newsItem.LastChangeDate
                };

                var files = new List<FileViewModel>();

                if (newsItem.Files != null)
                {
                    foreach (var newsItemFile in newsItem.Files)
                    {
                        files.Add(new FileViewModel()
                        {
                            Id = newsItemFile.Id,
                            Name = newsItemFile.OriginalName
                        });
                    }
                }

                newsItemViewModel.Files = files;

                records.Add(newsItemViewModel);
            }

            var list = new PagedList<NewsItemViewModel>(records, pageIndex, pageSize);
            var model = new NewsListViewModel
            {
                CategoryId = category,
                News = list,
                PagerModel = new PagerViewModel
                {
                    PageSize = list.PageSize,
                    TotalRecords = list.TotalCount,
                    PageIndex = list.PageIndex,
                    RouteValues = new RouteValues { page = pageIndex, category = category },
                }
            };
            return model;
        }
    }
}
