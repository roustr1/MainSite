using Application.Services.PlanCalendar;
using MainSite.Areas.Admin.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlanCalendarController : Controller
    {
        private readonly IPlanCalendarSevice _planCalendarSevice;
        private readonly IPlanCalendarFactory _planCalendarFactory;

        public PlanCalendarController(IPlanCalendarSevice planCalendarSevice, IPlanCalendarFactory planCalendarFactory)
        {
            _planCalendarSevice = planCalendarSevice;
            _planCalendarFactory = planCalendarFactory;
        }
        [Route("Admin/PlanCalendar/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Admin/PlanCalendar/Create")]
        [HttpPost]
        public IActionResult Create(IFormFile fileCalendar)
        {
            if(ModelState.IsValid && fileCalendar != null)
            {
                var collection = _planCalendarFactory.Start(fileCalendar);
                foreach (var item in collection)
                {
                    _planCalendarSevice.CreatePlanCalendar(_planCalendarFactory.GetEntity(item));
                }

                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
