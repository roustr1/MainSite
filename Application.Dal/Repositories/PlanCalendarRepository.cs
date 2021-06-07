using Application.Dal.Domain.PlanCalendar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Application.Dal
{
    public class PlanCalendarRepository :  EfRepository<PlanCalendar>
    {
        public PlanCalendarRepository(ApplicationContext context) : base(context)
        {

        }
        
        public  PlanCalendar GetLast()
        {
            var currentDate = DateTime.Today;
            return _context.PlanCalendars
                .OrderBy(c=>c.Year)
                .ThenBy(c => c.Month)
                .Include(a => a.Events)
                .LastOrDefault(s => s.Month == currentDate.Month && currentDate.Year == s.Year);
        }
    }
}
