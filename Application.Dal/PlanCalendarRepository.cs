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
        
        public PlanCalendar Get()
        {
            var currentDate = DateTime.Now;
            return _context.PlanCalendars.Where(s => s.Month == currentDate.Month && currentDate.Year == s.Year).Include(a => a.Events).ToList().LastOrDefault();
        }
    }
}
