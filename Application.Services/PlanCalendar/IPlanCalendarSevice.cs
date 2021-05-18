using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Services.PlanCalendar
{
    public interface IPlanCalendarSevice
    {
        List<DataBaseParserModel> Start(IFormFile file);
    }
}