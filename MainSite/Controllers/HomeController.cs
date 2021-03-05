using MainSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Application.Dal.Domain.News;
using Application.Services.Files;
using Application.Services.News;

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

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(NewsItemModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new NewsItem();
                entity.Header = model.Header;
                entity.Description = model.Description;

                //uploadFiles 
                foreach (var file in model.UploadedFiles)
                {
                    var file1 = _uploadService.InsertFile(file);
                }


            }
            return RedirectToAction("News", new { category = model.Category });
        }




        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return Error();
            var item = _newsService.GetNewsItem(id);
            if (item == null) return Error();
            _newsService.DeleteNews(item);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
