using Microsoft.AspNetCore.Mvc;
using MainSite.Controllers;
using Newtonsoft.Json;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public abstract partial class BaseAdminController : BaseController
    {
        /// <summary>
        /// Creates an object that serializes the specified object to JSON.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <returns>The created object that serializes the specified data to JSON format for the response.</returns>
        public override JsonResult Json(object data)
        {
            //use IsoDateFormat on writing JSON text to fix issue with dates in grid
            var serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };
            return base.Json(data, serializerSettings);
        }
    }
}
