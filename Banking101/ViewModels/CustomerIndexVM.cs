using Banking101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.ViewModels
{
    public class CustomerIndexVM
    {
        public PagedList<Customer> CustomersPageList { get; set; }

        public Customer SelectedCustomer { get; set; }

        public string SelectedCountry { get; set; }

        public Dictionary<string, string> AllCountries { get; set; }
    }
}
