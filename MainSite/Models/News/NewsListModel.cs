using System.Collections.Generic;
using Application.Dal.Domain.News;
using MainSite.Models.Common;
using MainSite.Models.UI.Menu;

namespace MainSite.Models.News
{
    public partial class NewsListModel
    {
        public IList<MenuItemViewModel> Menu { get; set; }
        public IList<NewsItem> News { get; set; }
        public PagerModel PagerModel { get; set; }
        public string CategoryId { get; set; }
    }
}