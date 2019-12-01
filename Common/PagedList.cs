using System.Collections.Generic;

namespace Common
{
    public class PagedList<T> : IPaged
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public long Total { get; set; }
        public long TotalPages
        {
            get
            {
                var totalPages = Total / PageSize;
                if (Total % PageSize > 0)
                {
                    totalPages++;
                }
                return totalPages;
            }
        }
        public IEnumerable<T> Items { get; set; }
    }
}