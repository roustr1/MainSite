using MainSite.Models;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiNewsController : ControllerBase
    {
        private readonly MainModel _mainMode;

        private static int _pagesize;

        public ApiNewsController(MainModel mainModel)
        {
            _mainMode = mainModel;

            SetPageSize();
        }

        private void SetPageSize()
        {
            _pagesize = _mainMode.GetSettingNewsPerPage();
        }

        [Route("newsItems")]
        public string MenuTreeGenerate(string category, int page = 0)
        {
            var model = _mainMode.GetNewsListViewModel(page, _pagesize, category);

            return JsonConvert.SerializeObject(model);
        }
     

    }
}
