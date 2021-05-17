using Application.Services.Birthday;
using Application.Services.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace MainSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        private readonly IBirthdayService _birthdayService;
        private readonly ISettingsService _settingsService;


        public ApiUsersController(IBirthdayService birthdayService, ISettingsService settingsService)
        {
            _birthdayService = birthdayService;
            _settingsService = settingsService;
        }

        [Route("GetBirthdayUsers")]
        public string GetBirthdayUsers()
        {
            string path = null;

            _settingsService?.SettingsDictionary.TryGetValue("BirthdayPath", out path);
            if (path != null && !string.IsNullOrEmpty(path))
            {
                var model = _birthdayService.GetUsers(path).ToList();
                if (model.Any())
                {
                    var result = JsonConvert.SerializeObject(model);

                    ///Исправление структуры данных модели представления
                    result = result.Replace("FullFio", "Fio");                 
                    result = result.Replace("PhotoPath", "Photo");

                    return result;
                }
            }

            return "[]";
        }
        [Route("InfoCurrentUser")]
        public string GetInfoCurrentUser()
        {
            var model = new
            {
                Name = User.Identity.Name,
                IsEditer = User.IsInRole("Модератор") || User.IsInRole("Администратор")
            };

            return JsonConvert.SerializeObject(model);
        }
    }
}
