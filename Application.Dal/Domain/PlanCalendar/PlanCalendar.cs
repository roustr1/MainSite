using System.Collections.Generic;

namespace Application.Dal.Domain.PlanCalendar
{
    public class PlanCalendar : BaseEntity
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public virtual ICollection<EventCalendar> Events { get; set; }
    }
}
