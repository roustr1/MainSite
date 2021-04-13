using Application.Services.Birthday;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MainSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        private readonly IBirthdayService _birthdayService;


        public ApiUsersController(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }

        [Route("GetBirthdayUsers")]
        public string GetBirthdayUsers()
        {

            return JsonConvert.SerializeObject(_birthdayService.GetUsers());
        }
    }
}
