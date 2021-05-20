using System.Collections.Generic;

namespace Application.Services.PlanCalendar
{
    public interface IPlanCalendarSevice
    {
        void CreatePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item);
        void UpdatePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item);
        void DeletePlanCalendar(Dal.Domain.PlanCalendar.PlanCalendar item);
        Dal.Domain.PlanCalendar.PlanCalendar GetPlanCalendar(string itemId);
        Dal.Domain.PlanCalendar.PlanCalendar GetLastPlanCalendar();
    }
}