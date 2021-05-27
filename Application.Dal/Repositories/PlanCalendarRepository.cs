using Application.Dal.Domain.PlanCalendar;
using Microsoft.EntityFrameworkCore;
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
            return _context.PlanCalendars.Include(a => a.Events).ToList().LastOrDefault();
        }
    }
}
