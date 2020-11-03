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
        public int NumberOfPages { get; set; }
        public List<T> Data { get; set; }
    }
}
