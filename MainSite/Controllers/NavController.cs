﻿using System;
using System.Collections.Generic;
using MainSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Services.Files;
using Application.Services.Menu;
using Application.Services.News;
using Application.Services.Settings;
using MainSite.Models.Common;
using MainSite.Models.News;
using MainSite.Models.UI.Menu;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MainSite.Controllers
{
    public class NavController : Controller
    {
        private IMenuService _service;
        public NavController(IMenuService service)
        {
            _service = service;
        }

        public PartialViewResult Menu(string categoryId = null)
        {
            ViewBag.SelectedCategory = categoryId;

            var menuItems = MenuTreeGenerate(categoryId);

            return PartialView("Menu", menuItems);
        }

        private IEnumerable<MenuItemViewModel> MenuTreeGenerate(string categoryId = null)
        {
            var result = new List<MenuItemViewModel>();

            foreach (var menuItem in _service.GetMenuItem())
            {
                if (menuItem != null)
                {
                    result.Add(new MenuItemViewModel()
                    {
                        Id = menuItem.Id,
                        Name = menuItem.Name,
                        ToolTip = menuItem.ToolTip,
                        UrlIcon = menuItem.UrlIcone,
                        IsActive = menuItem.Id == categoryId
                    });
                }
            }

            return result;
        }
    }
}