using Microsoft.AspNetCore.Mvc; 

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [Route("Admin/Index")]
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
    }
}
