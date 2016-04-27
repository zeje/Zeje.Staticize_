using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeje.Staticize_.AppCode
{
    public class SimplePageObj<T> where T : new()
    {

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public int totalCount { get; set; }

        public List<T> rows { get; set; }

        public bool prePage
        {
            get
            {
                return pageIndex > 0;
            }
        }
        public bool nextPage
        {
            get
            {
                return (pageIndex + 1) * pageSize < totalCount;
            }
        }
        public int prePageIndex
        {
            get
            {
                return pageIndex <= 1 ? 0 : pageIndex - 1;
            }
        }
        public int nextPageIndex
        {
            get
            {
                return pageIndex + 1;
            }
        }
    }
}