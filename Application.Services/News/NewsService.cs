using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain;
using Application.Dal.Domain.News;
using Application.Services.Utils;

namespace Application.Services.News
{
 

    public class NewsService : INewsService
    {
        private readonly IRepository<NewsItem> _newsRepository;

        public NewsService(IRepository<NewsItem> newsRepository)
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
        /// <remarks> Поиск по дате осуществляется только в период с<paramref name= "startDate" />
        /// по < paramref name= "endDate" /></remarks> 
        /// <param name="authorId">Автор</param>
        /// <param name="category">Категория</param>
        /// <param name="startDate">С даты</param>
        /// <param name="endDate">По дату</param>
        public IEnumerable<NewsItem> GetNewsItem(string authorId = null, string category = null,
            DateTime? startDate = null, DateTime? endDate = null, bool isNewest = true)
        {
            var collection = _newsRepository.GetAll()
                .Where(a => category == null || a.Category == category)
                .Where(a => authorId == null || a.AutorFio == authorId || a.LastChangeAuthor == authorId)
                .Where(a => startDate == null || a.LastChangeDate >= startDate)
                .Where(a => endDate == null || a.LastChangeDate <= endDate)
                .SortByNewestOrOldest(isNewest, item => item.LastChangeDate);
            
            return collection.AsQueryable();
        }

 
        #endregion
    }
}
