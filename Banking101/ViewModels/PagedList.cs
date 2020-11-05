using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.ViewModels
{
    public class PagedList<T>
    {
        public int Page { get; set; }
        public int RowsPerPage { get; set; }

        public int TotalCount { get; set; }
        public int NumberOfPages => (int)Math.Ceiling((decimal)TotalCount / RowsPerPage);

        public List<T> Data { get; set; }
    }
}
