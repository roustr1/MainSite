using System.Net;
using Microsoft.AspNetCore.Mvc;

using Application.Services.Files;
using Microsoft.AspNetCore.Http;

namespace MainSite.Controllers
{
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;
        private readonly IFileDownloadService _downloadService;

        public PictureController(IPictureService pictureService, IFileDownloadService downloadService)
        {
            _pictureService = pictureService;
            _downloadService = downloadService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile Upload)
        {
            if (Upload != null)
            {

                var pic = _pictureService.InsertPicture(Upload, "photos");
                var url = _pictureService.GetPictureUrl(pic.Id);

                return Json(new
                {
                    success = true,
                    downloadUrl = url
                });
            }
            return Json(new
            {
                success = false,
                error = "File not selected"
            });
        }

        public IActionResult DownloadPicture(string downloadId)
        {
            var pic = _pictureService.GetPictureUrl(downloadId);
            return Json(new
            {
                success = true,
                downloadUrl = pic
            });
        }
    }
}
