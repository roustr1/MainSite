using System.Collections.Generic;

namespace MainSite.Models
{
    public class ParserDateModel
    {
        public int day { get; set; }
        public int lastDay { get; set; }
        public string Text { get; set; }
        public bool IsExist { get; set; }
    }

    public class EventCalendarModel
    {
        public string Day { get; set; }
        public string NameProgram { get; set; }
        public string Name { get; set; }
        public string NameAllStav { get; set; }
        public string Leader { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string Result { get; set; }

        public EventCalendarModel()
        {
        }
    }

    public class PlanCalendarModel
    {
        public List<EventCalendarModel> Events { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }

        public PlanCalendarModel()
        {
            Events = new List<EventCalendarModel>();
        }
    }
}
