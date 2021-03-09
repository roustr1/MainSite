using Application.Dal.Domain.Files;
using Application.Services.Files;
using Application.Services.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainSite.Controllers
{
    public class NewsController : Controller
    {
        #region Fields

        private readonly INewsService _newsService;
        private readonly IFileDownloadService _fileService;

        #endregion

        #region Ctor
        public NewsController(INewsService newsService, IFileDownloadService fileService)
        {
            _newsService = newsService;
            _fileService = fileService;
        }
        #endregion

        public ViewResult Index(string menuName)
        {
            var model = _newsService.GetNewsItem(menuName);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(NewsModel model, IFormFile[] uploadedFiles)
        //{
        //    var download = new File
        //    {
                
        //    };
        //    return View();
        //}



    }
}
