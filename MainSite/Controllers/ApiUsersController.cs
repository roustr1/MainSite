using Application.Services.Birthday;
using Application.Services.Settings;
using Application.Services.Users;
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
        private readonly IUsersService _userService;


        public ApiUsersController(IBirthdayService birthdayService, ISettingsService settingsService, IUsersService userService)
        {
            _birthdayService = birthdayService;
            _settingsService = settingsService;
            _userService = userService;
        }

        [Route("GetBirthdayUsers")]
        public string GetBirthdayUsers()
        {
            string path = null;

            _settingsService?.SettingsDictionary.TryGetValue("BirthdayPath", out path);
            if (path != null && !string.IsNullOrEmpty(path))
            {
                var model = _birthdayService.GetTodayBirth().ToList();
                if (model.Any())
                {
                    return JsonConvert.SerializeObject(model); 
                }
            }

            return "[]";
        }
        [Route("InfoCurrentUser")]
        public string GetInfoCurrentUser()
        {
            var model = new
            {
                Name = _userService.GetUserBySystemName(User)?.Name,
                IsEditer = User.IsInRole("Модератор") || User.IsInRole("Администратор")
            };

            return JsonConvert.SerializeObject(model);
        }
    }
}
