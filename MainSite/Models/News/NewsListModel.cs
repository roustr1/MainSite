using System.Collections.Generic;
using Application.Dal.Domain.News;
using MainSite.Models.Common;

namespace MainSite.Models.News
{
    public partial class NewsListModel
    {
        public IList<NewsItem> News { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}