using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivingRoom.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public long Count { get; private set; }

        private PagedResult()
        {
        }

        public PagedResult(IEnumerable<T> results, int pageNumber, int pageSize, long count)
        {
            Results = results;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Count = count;
        }
    }
}