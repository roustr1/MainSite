using MainSite.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace MainSite.Areas.Admin.Components.UserRoles
{
    public class UserRoleCard : ViewComponent
    {
        public IViewComponentResult Invoke(UserRoleModel model)
        {
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View("Default", model);
        }
    }
}