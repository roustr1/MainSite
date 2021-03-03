using System;
using System.Collections.Generic;
using Application.Dal.Domain.News;

namespace Application.Services.News
{
    public interface INewsService
    {
        void CreateNews(NewsItem item);
        void UpdateNews(NewsItem item);
        void DeleteNews(NewsItem item);
        NewsItem GetNewsItem(string itemId);

        IEnumerable<NewsItem> GetNewsItem(string authorId = null, string menuName = null,
            DateTime? startDate = null, DateTime? endDate = null);
    }
}