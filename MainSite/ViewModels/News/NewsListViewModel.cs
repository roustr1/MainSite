using System.Collections.Generic;
using Application.Dal.Domain.News;
using MainSite.ViewModels.Common;
using MainSite.ViewModels.UI.Menu;

namespace MainSite.ViewModels.News
{
    public partial class NewsListViewModel
    {
        public IList<MenuItemViewModel> Menu { get; set; }
        public IList<NewsItem> News { get; set; }
        public PagerViewModel PagerModel { get; set; }
        public string CategoryId { get; set; }
    }
}