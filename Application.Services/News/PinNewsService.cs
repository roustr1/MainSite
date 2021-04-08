using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dal;
using Application.Dal.Domain.News;

namespace Application.Services.News
{
    public class PinNewsService
    {
        private readonly IRepository<PinNews> _newsRepository;

        public PinNewsService(IRepository<PinNews> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public void PinNews(PinNews pinNews)
        {
            _newsRepository.Add(pinNews);
        }

        public void PinNews(string categoryId, string newsId)
        {
            var pn = new PinNews
            {
                CategoryId = categoryId, 
                NewsItemId = newsId, 
                Index = GetMaximumIndexForCategory(categoryId)
            };

            PinNews(pn);
        }

        public void UnpinNews(string newsId)
        {
            var pn = _newsRepository.Get(newsId);

            if(pn != null)
            {
                UnpinNews(pn);
            }
        }

        public void UnpinNews(PinNews pinNews)
        {
            _newsRepository.Delete(pinNews);
        }

        public IEnumerable<PinNews> GetAllPinnedNewsByCategory(string categoryId)
        {
            return _newsRepository.GetMany(p => p.CategoryId == categoryId).ToList();
        }

        private int GetMaximumIndexForCategory(string categoryId)
        {
            var result = 0;
            
            foreach (var pinnedNews in _newsRepository.GetMany(p => p.CategoryId == categoryId).ToList())
            {
                if(pinnedNews == null) continue;

                if (pinnedNews.Index > result)
                {
                    result = pinnedNews.Index;
                }
            }

            return result;
        }
    }
}
