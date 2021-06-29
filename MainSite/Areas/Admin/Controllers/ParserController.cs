using Application.Services.Birthday;
using Application.Services.PlanCalendar;
using MainSite.Areas.Admin.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ParserController : Controller
    {
        private readonly IPlanCalendarSevice _planCalendarSevice;
        private readonly IPlanCalendarFactory _planCalendarFactory;
        private readonly IBirthdayFactory _birthdayFactory;
        private readonly IBirthdayService _birthdayService;

        public ParserController(IPlanCalendarSevice planCalendarSevice, IPlanCalendarFactory planCalendarFactory, IBirthdayFactory birthdayFactory, IBirthdayService birthdayService)
        {
            _planCalendarSevice = planCalendarSevice;
            _planCalendarFactory = planCalendarFactory;
            _birthdayFactory = birthdayFactory;
            _birthdayService = birthdayService;

        }
        [Route("Admin/PlanCalendar/Index")]
        [HttpGet]
        public IActionResult IndexPlanCalendar()
        {
            return View();
        }

        [Route("Admin/Birthday/Index")]
        [HttpGet]
        public IActionResult IndexBirthday()
        {
            return View();
        }
        

        [Route("Admin/PlanCalendar/Create")]
        [HttpPost]
        public IActionResult CreatePlanCalendar(IFormFile fileCalendar)
        {
            if(ModelState.IsValid && fileCalendar != null)
            {
                var collection = _planCalendarFactory.ParseFile(fileCalendar);
                foreach (var item in collection)
                {
                    _planCalendarSevice.CreatePlanCalendar(_planCalendarFactory.GetEntity(item));
                }

                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }

        [Route("Admin/Birthday/Create")]
        [HttpPost]
        public IActionResult CreateBirthday(IFormFile fileCalendar)
        {
            if (ModelState.IsValid && fileCalendar != null)
            {
                var collection = _birthdayFactory.ParseFile(fileCalendar);

                foreach(var item in collection)
                {
                    _birthdayService.AddItem(item);
                }

                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
