using System.Collections.Generic;
using System.Linq;
using Application.Dal.Domain.News;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal
{
    public class NewsItemRepository : EfRepository<NewsItem>
    {
        public NewsItemRepository(ApplicationContext context) : base(context)
        {
        }

        public override NewsItem Get(string id)
        {
            return _context.NewsItems.Include(a => a.Files).ToList().LastOrDefault();
        }

        public override IEnumerable<NewsItem> GetAll
        {
            get { return _context.NewsItems.Include(a => a.Files); }
        }

        public IEnumerable<NewsItem> GetCollection( int skip, int take, out int totalCount)
        {
            var news = _context.NewsItems.Include(a => a.Files).Skip(skip).Take(take);
            totalCount = _context.NewsItems.Count();
            return news;
        }
    }
}