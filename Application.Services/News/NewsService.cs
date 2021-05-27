using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.News;
using Application.Services.Infrastructure;
using Application.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.News
{
 

    public class NewsService : INewsService
    {
        private readonly NewsItemRepository _newsRepository;

        public NewsService(NewsItemRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        #region Crud

        public void CreateNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Add(item);
        }

        public void UpdateNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Update(item);

        }


        public void DeleteNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Delete(item);

        }

        public NewsItem GetNewsItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _newsRepository.Get(itemId);
        }

        /// <summary>
        /// Поиск новостей 
        /// </summary>
        public IEnumerable<NewsItem> GetNewsItem(FilterNewsItemParameters filterNewsItemParameters)
        {
            var category = filterNewsItemParameters.CategoryIds.FirstOrDefault();
            var categories = filterNewsItemParameters.CategoryIds.ToList();
            var authorId = filterNewsItemParameters.AuthorId;
            var startDate = filterNewsItemParameters.StartDate;
            var endDate = filterNewsItemParameters.EndDate;
            var isNewest = filterNewsItemParameters.IsNewest;
            var pinnedNews = filterNewsItemParameters.PinnedNewsIds;

            var collection = _newsRepository.GetAll
                .Where(a => category == null || categories.Contains(a.Category))
                .Where(a => !pinnedNews.Contains(a.Id))
                .Where(a => authorId == null || a.AutorFio == authorId || a.LastChangeAuthor == authorId)
                .Where(a => startDate == null || a.LastChangeDate >= startDate)
                .Where(a => endDate == null || a.LastChangeDate <= endDate)
                .SortByNewestOrOldest(isNewest, item => item.LastChangeDate);
            
            return collection.AsQueryable();
        }

        public IEnumerable<NewsItem> FindFreeText(string query)
        {
            var collection = _newsRepository.GetMany(x => EF.Functions.FreeText(x.Description, query));

            return collection;
        }
        #endregion
    }
}
