using Application.Services.PlanCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlanCalendarController : Controller
    {
        private readonly IPlanCalendarSevice _planCalendarSevice;

        public PlanCalendarController(IPlanCalendarSevice planCalendarSevice)
        {
            _planCalendarSevice = planCalendarSevice;
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
                var collection = _planCalendarSevice.Start(fileCalendar);
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
