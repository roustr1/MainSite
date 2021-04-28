using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.ViewModels.News
{
    public class PinnedNewsViewModel
    {
        public int Index { get; set; }
        public NewsItemViewModel NewsItem { get; set; }
    }
}
