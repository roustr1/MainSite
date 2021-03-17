using System;
using System.Collections.Generic;
using MainSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

namespace MainSite.Components
{
    [ViewComponent(Name = "NavComponent")]
    public class NavViewComponent : ViewComponent
    {
        private IMenuService _service;
        public NavViewComponent(IMenuService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke(string categoryId = null)
        {
            var menuItems = MenuTreeGenerate(categoryId);

            return View(menuItems);
        }

        private IEnumerable<MenuItemViewModel> MenuTreeGenerate(string categoryId = null)
        {
            var result = new List<MenuItemViewModel>();

            var selectedCategory = GenerateAllSelected(categoryId);

            CreateTree(null, result, selectedCategory);

            return result;
        }

        private List<string> GenerateAllSelected(string id)
        {
            var localId = id;
            var result = new List<string>();

            while (localId != null)
            {
                MenuItem menuItem = _service.Get(localId);
                if (menuItem != null)
                {
                    result.Add(menuItem.Id);
                    localId = menuItem.ParentId;
                }
                else
                {
                    localId = null;
                }
            }

            return result;
        }

        private void CreateTree(string id, List<MenuItemViewModel> list, List<string> selected)
        {
            foreach (var item in _service.GetManyByParentId(id))
            {
                var itemViewModel = new MenuItemViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ToolTip = item.ToolTip,
                    UrlIcon = item.UrlIcone,
                    IsActive = selected.Contains(item.Id)
                };

                CreateTree(item.Id, itemViewModel.Children, selected);

                list.Add(itemViewModel);
            }
        }
    }
}