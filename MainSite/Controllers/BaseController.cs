using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace MainSite.Controllers
{
    public abstract class BaseController : Controller
    {

        #region Security

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Accexzss denied view</returns>
        internal ActionResult AccessDeniedView() 
            => RedirectToAction("AccessDenied", "Security", new { pageUrl = GetRawUrl(Request) });


        /// <summary>
        /// Access denied JSON data for DataTables
        /// </summary>
        /// <returns>Access denied JSON data</returns>
        protected JsonResult AccessDeniedDataTablesJson()
        {
            return ErrorJson("Admin.AccessDenied.Description");
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Error's JSON data
        /// </summary>
        /// <param name="error">Error text</param>
        /// <returns>Error's JSON data</returns>
        protected JsonResult ErrorJson(string error)
        {
            return Json(new
            {
                error = error
            });
        }

        /// <summary>
        /// Error's JSON data
        /// </summary>
        /// <param name="errors">Error messages</param>
        /// <returns>Error's JSON data</returns>
        protected JsonResult ErrorJson(object errors)
        {
            return Json(new
            {
                error = errors
            });
        }

        #endregion

        [NonAction]
        public virtual string GetRawUrl(HttpRequest request)
        {
            //first try to get the raw target from request feature
            //note: value has not been UrlDecoded
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //or compose raw URL manually
            if (string.IsNullOrEmpty(rawUrl))
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";

            return rawUrl;
        }

    }
}