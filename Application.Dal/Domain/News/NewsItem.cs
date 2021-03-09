using System.Collections.Generic;
using Application.Dal.Domain.Files;

namespace Application.Dal.Domain.News
{
    public class NewsItem : BaseEntity
    {
        public NewsItem()
        {
            Files = new List<File>();
        }
 
        public string Header { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual ICollection<File> Files { get; set; }

    }
}