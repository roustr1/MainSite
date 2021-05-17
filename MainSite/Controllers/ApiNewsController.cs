﻿using MainSite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


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
        [HttpPost]
        public string MenuTreeGenerate(string category, int page = 0)
        {
            var model = _mainMode.GetNewsListViewModel(page, _pagesize, category);

            return JsonConvert.SerializeObject(model);
        }

        [Route("search")]
        [HttpPost]
        public string SearchNews(string search)
        {
            var result = _mainMode.GetManySearchResultNewsItemViewModel(search);

            return JsonConvert.SerializeObject(result);

        }
    }
}
