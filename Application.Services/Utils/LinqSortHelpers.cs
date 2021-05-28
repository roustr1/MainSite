using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Services.Utils
{
    public static class LinqSortHelpers
    {
        public static IOrderedEnumerable<TSource> SortByNewestOrOldest<TSource, TKey>(
            this IEnumerable<TSource> source,
            bool newest,
            Func<TSource, TKey> keySelector)
        {
            return newest ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }
    }
}
