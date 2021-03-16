using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.Models.UI.Menu
{
    public class MenuItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UrlIcon { get; set; }
        public string ToolTip { get; set; }

        public List<MenuItemViewModel> Children { get; set; }

        public MenuItemViewModel()
        {
            Children = new List<MenuItemViewModel>();
        }
    }
}
