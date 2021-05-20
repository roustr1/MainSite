using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dal.Domain.PlanCalendar
{
    public class EventCalendar: BaseEntity
    {
        public string Day { get; set; }
        public string NameProgram { get; set; }
        public string Name { get; set; }
        public string NameAllStav { get; set; }
        public string Leader { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string Result { get; set; }
        public string PlanCalendarId { get; set; }
    }
}