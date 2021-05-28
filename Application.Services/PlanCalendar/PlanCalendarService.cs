using Application.Dal;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.PlanCalendar
{
    public class PlanCalendarSevice: IPlanCalendarSevice
    {
        private readonly PlanCalendarRepository _planCalendarRepository;

        public PlanCalendarSevice(PlanCalendarRepository planCalendarRepository)
        {
            _planCalendarRepository = planCalendarRepository;
        }

        public void CreatePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item)
        {
            if(item != null)
                _planCalendarRepository.Add(item);
        }

        public void UpdatePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item)
        {
            _planCalendarRepository.Update(item);
        }

        public void DeletePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item)
        {
            _planCalendarRepository.Delete(item);
        }

        public Dal.Domain.PlanCalendar.PlanCalendar GetPlanCalendar(string itemId)
        {
            return _planCalendarRepository.Get(itemId);        
        }

        public Dal.Domain.PlanCalendar.PlanCalendar GetLastPlanCalendar()
        {
            return _planCalendarRepository.GetLast();
        }
    }
}
