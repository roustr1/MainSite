using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dal;
using Application.Dal.Domain.News;

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


        public  IEnumerable<NewsItem> GetNewsItem(string authorId = null, string menuName = null,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            var collection =  _newsRepository.GetAll();
            if (authorId != null) collection = collection.Where(a => a.lastChangeAuthor == authorId);
            if (menuName != null) collection = collection.Where(c => c.MenuName == menuName);
            if (startDate != null)
            {
                collection = collection.Where(c => c.CreatedDate >= startDate || c.LastChangeDate >= startDate);
                if (endDate != null)
                {
                    collection = collection.Where(c => c.CreatedDate <= endDate || c.LastChangeDate <= endDate);
                }
            }
            return collection.ToList();
        }
        #endregion
    }
}
