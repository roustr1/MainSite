using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.ViewModels.UI.Menu
{
    public class MenuItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UrlIcon { get; set; }
        public string ToolTip { get; set; }
        public string ParentId { get; set; }
        public bool IsActive { get; set; }
        public List<MenuItemViewModel> Children { get; set; }
        public int Index { get; set; }
        public MenuItemViewModel()
        {
            Children = new List<MenuItemViewModel>();
        }
    }
}
