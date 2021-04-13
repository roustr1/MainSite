using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using MainSite.ViewModels.UI.Menu;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MainSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMenuController : ControllerBase
    {
        private IMenuService _service;

        public ApiMenuController(IMenuService menuService)
        {
            _service = menuService;
        }
        [Route("categories")]
        public IEnumerable<MenuItemViewModel> MenuTreeGenerate(string categoryId = null)
        {
            var result = new List<MenuItemViewModel>();

            var selectedCategory = GenerateAllSelected(categoryId);

            CreateTree(null, result, selectedCategory);

            return result;
        }

        public List<string> GenerateAllSelected(string id)
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
            foreach (var item in _service.GetManyByParentId(id).OrderBy(i => i.Index))
            {
                var itemViewModel = new MenuItemViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ToolTip = item.ToolTip,
                    UrlIcon = item.UrlIcone,
                    IsActive = selected.Contains(item.Id),
                    Index = item.Index

                };

                list.Add(itemViewModel);
            }

            foreach (var item in list)
            {
                CreateTree(item.Id, item.Children, selected);
            }
        }
    }
}
