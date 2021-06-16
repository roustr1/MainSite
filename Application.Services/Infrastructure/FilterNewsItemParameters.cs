using System;
using System.Collections.Generic;

namespace Application.Services.Infrastructure
{
    public class FilterNewsItemParameters
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
        public string AuthorId { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public bool IsNewest { get; set; } = true;
        public IEnumerable<string> CategoryIds { get; set; } = new List<string>();
        public IEnumerable<string> PinnedNewsIds { get; set; } = new List<string>();
    }
}
