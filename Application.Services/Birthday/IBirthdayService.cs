using System.Collections.Generic;
using Application.Dal.Domain.Birthday;

namespace Application.Services.Birthday
{
    public interface IBirthdayService
    {
         IEnumerable<Birtday> GetTodayBirth();
    }
}
