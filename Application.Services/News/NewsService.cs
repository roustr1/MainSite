using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.News;
using Application.Dal.Repositories.Infrastructure;
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
        public IEnumerable<NewsItem> GetNewsItem(FilterNewsItemParameters filterNewsItemParameters) =>
                                _newsRepository.GetFiltered(filterNewsItemParameters);




        public IEnumerable<NewsItem> FindFreeText(string query) =>

                                 _newsRepository.GetMany(x => EF.Functions.FreeText(x.Description, query)
                                                              || EF.Functions.FreeText(x.Header, query))
                                     .OrderByDescending(c => c.CreatedDate);




        public int GetTotalNewsCount(string categoryId = null) =>
                     _newsRepository.GetNewsCount(categoryId);


        #endregion
    }
}
