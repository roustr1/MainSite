using Application.Services.PlanCalendar;
using MainSite.Areas.Admin.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiPlanCalendarController : ControllerBase
    {
        private readonly IPlanCalendarSevice _planCalendarSevice;
        private readonly IPlanCalendarFactory _planCalendarFactory;

        public ApiPlanCalendarController(IPlanCalendarSevice planCalendarSevice, IPlanCalendarFactory planCalendarFactory)
        {
            _planCalendarFactory = planCalendarFactory;
            _planCalendarSevice = planCalendarSevice;
        }

        [Route("getPlanCalendar")]
        [HttpPost]
        public IActionResult GetPlanCalendar()
        {
            var collection = _planCalendarSevice.GetLastPlanCalendar();

            return new JsonResult(collection);
        }
    }
}
