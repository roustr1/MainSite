using System;
using System.Collections.Generic;

namespace Application.Dal.Repositories.Infrastructure
{
    public class FilterNewsItemParameters
    {

        public int Skip { get; set; }
        public int Take { get; set; }
        public string AuthorId { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public bool? IsNewest { get; set; }
        public IEnumerable<string> CategoryIds { get; set; } = new List<string>();
        public IEnumerable<string> PinnedNewsIds { get; set; } = new List<string>();
    }
}
