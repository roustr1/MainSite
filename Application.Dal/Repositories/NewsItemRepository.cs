using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return _context.NewsItems.Include(a => a.Files).FirstOrDefault(c=>c.Id==id);
        }

        public override IEnumerable<NewsItem> GetAll
        {
            get { return _context.NewsItems.Include(a => a.Files); }
        }

        public override IEnumerable<NewsItem> GetMany(Expression<Func<NewsItem, bool>> @where)
        {
            return _context.Set<NewsItem>().Include(a=>a.Files).Where(@where);
        }


    }
}