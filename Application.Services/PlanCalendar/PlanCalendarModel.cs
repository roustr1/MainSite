using System.Collections.Generic;

namespace Application.Services.PlanCalendar
{
    public class ParserDateModel
    {
        public int day { get; set; }
        public int lastDay { get; set; }
        public string Text { get; set; }
        public bool IsExist { get; set; }
    }

    public class ParserActivityModel
    {
        public string DayOfEvent { get; set; }
        public string NameProgram { get; set; }
        public string Name { get; set; }
        public string NameAllStav { get; set; }
        public string Leader { get; set; }
        public string LocationOfEvent { get; set; }
        public string TimeOfEvent { get; set; }
        public string ResultOfEvent { get; set; }

        public ParserActivityModel()
        {
        }
    }

    public class DataBaseParserModel
    {
        public List<ParserActivityModel> ParserActivityModelCollection { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }

        public DataBaseParserModel()
        {
            ParserActivityModelCollection = new List<ParserActivityModel>();
        }
    }
}
