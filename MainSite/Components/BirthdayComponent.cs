using System.Linq;
using System.Threading.Tasks;
using Application.Services.Birthday;
using Application.Services.Settings;
using Microsoft.AspNetCore.Mvc;

namespace MainSite.Components
{
    public class BirthdateComponent : ViewComponent
    {
        private readonly IBirthdayService _birthdayService;
        private readonly ISettingsService _settingsService;

        public BirthdateComponent(IBirthdayService birthdayService, ISettingsService settingsService)
        {
            _birthdayService = birthdayService;
            _settingsService = settingsService;
        }
        public IViewComponentResult Invoke()
        {
            string path = null;

            _settingsService?.SettingsDictionary.TryGetValue("BirthdayPath", out path);
            if (path != null && !string.IsNullOrEmpty(path))
            {
                var model = _birthdayService.GetUsers(path).ToList();
                if (model.Any())
                {
                    return View("Birthdate", model);
                }
            }

            return Content(string.Empty);
        }
    }
}