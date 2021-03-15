using System;
using MainSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Application.Dal.Domain.News;
using Application.Services.Files;
using Application.Services.News;
using Microsoft.EntityFrameworkCore;

namespace MainSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IFileDownloadService _downloadService;
        private readonly IFileUploadService _uploadService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, IFileDownloadService downloadService, IFileUploadService uploadService)
        {
            _logger = logger;
            _newsService = newsService;
            _downloadService = downloadService;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            return View();
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

            return View();
        }

        //[Route("Create")]
        [HttpPost("{Create}")]
        public IActionResult Create([FromForm] NewsItemModel model)
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
                    Name =  model.Header,
                    Category =  model.Category,
                    UrlImg = "",
                    
                    

                };

                //uploadFiles 
                foreach (var file in model?.UploadedFiles)
                {
                    entity.Files.Add(_uploadService.InsertFile(file));
                }

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
            return View(model);
        }

        public IActionResult GetFile(string fileId)
        {
            if (fileId == null) return new EmptyResult();
            var fileBinary = _uploadService.GetFileBinaryByFileId(fileId);
            var file = _uploadService.GetFileById(fileId);
            return new FileContentResult(fileBinary.BinaryData, file.MimeType);
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
    }
}
