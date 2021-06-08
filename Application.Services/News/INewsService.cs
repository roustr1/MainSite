using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.News;
using Application.Services.Infrastructure;

namespace Application.Services.News
{
    public interface INewsService
    {
        void CreateNews(NewsItem item);
        void UpdateNews(NewsItem item);
        void DeleteNews(NewsItem item);

        NewsItem GetNewsItem(string itemId);
        IEnumerable<NewsItem> GetNewsItem(FilterNewsItemParameters filterNewsItemParameters);
        IEnumerable<NewsItem> FindFreeText(string query);
    }
}