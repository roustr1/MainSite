using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Dal.Domain.News;
using Application.Dal.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Application.Dal
{
    public class NewsItemRepository : EfRepository<NewsItem>
    {
        public NewsItemRepository(ApplicationContext context) : base(context)
        {
        }

        public override NewsItem Get(string id)
        {
            return _context.NewsItems.FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<NewsItem> GetAllQueryable => _context.NewsItems;


        public override IEnumerable<NewsItem> GetMany(Expression<Func<NewsItem, bool>> @where)
        {
            return _context.Set<NewsItem>().Include(newsItem => newsItem.Files).Where(@where);
        }

        public int GetNewsCount(string categoryId = null)
        {
            if (categoryId == null) return _context.NewsItems.Count();
            return _context.NewsItems.Count(c => c.Category == categoryId);
        }

        /// <summary>
        /// Фильтр для поиска среди новостей
        /// </summary>
        /// <param name="filterNewsItemParameters">параметры для фильтра, может быть null</param>
        /// <returns></returns>
        public IEnumerable<NewsItem> GetFiltered(FilterNewsItemParameters filterNewsItemParameters)
        {
            var category = filterNewsItemParameters?.CategoryIds?.FirstOrDefault() ?? null;
            var categories = filterNewsItemParameters?.CategoryIds.ToList() ?? null;
            var authorId = filterNewsItemParameters?.AuthorId;
            var startDate = filterNewsItemParameters?.StartDate;
            var endDate = filterNewsItemParameters?.EndDate;
            var isNewest = filterNewsItemParameters?.IsNewest;
            var pinnedNews = filterNewsItemParameters?.PinnedNewsIds;
            var skip = filterNewsItemParameters?.Skip ?? 0;
            var take = filterNewsItemParameters?.Take ?? 5;

            var data = GetAllQueryable;
            if (category != null)
            {

                data = data.Where(d => categories.Contains(d.Category));

            }
            if (isNewest == null)//по-умолчанию, сортировка от последнего к первому элементу
            {
                data = data.OrderByDescending(d => d.CreatedDate);
            }
            else//но её можно задать явно
            {
                data = isNewest.Value ? data.OrderByDescending(d => d.CreatedDate) : data.OrderBy(d => d.CreatedDate);
            }

            if (authorId != null)
            {
                data = data.Where(d => d.AutorFio == authorId);
            }

            if (startDate != null && startDate != DateTime.MinValue)
            {
                data = data.Where(d => d.CreatedDate >= startDate);
            }

            if (endDate != null && endDate != DateTime.MinValue)
            {
                data = data.Where(d => d.CreatedDate >= endDate);
            }
            if (pinnedNews != null && pinnedNews.Any())
            {
                data = data.Where(d => pinnedNews.Contains(d.Id));
            }

           

            if (skip != 0)
            {
                data = data.Skip(skip);
            }

            if (take != 0)
            {
                data = data.Take(take);
            }
            Console.WriteLine(data.ToQueryString());
            return data.Include(c => c.Files);
        }
    }
}