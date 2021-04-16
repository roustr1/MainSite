using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.News;
using MainSite.Models;

namespace MainSite.Controllers
{
    public class SearchController : Controller
    {
        private MainModel _mainModel;

        public SearchController(MainModel mainModel)
        {
            _mainModel = mainModel;
        }

        [Route("Search/Index")]
        public IActionResult Index(string textSearch)
        {
            return View(_mainModel.GetManySearchResultNewsItemViewModel(textSearch));
        }
    }
}
