using Application.Dal.Domain.Birthday;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MainSite.Areas.Admin.Factories
{
    public interface IBirthdayFactory
    {
        List<Birtday> ParseFile(IFormFile file);
    }
}
