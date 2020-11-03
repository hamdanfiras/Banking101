using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking101.Models;

namespace Banking101.ViewModels
{
    public class AccountListVM
    {
        public PagedList<Account> Accounts { get; set; }
        public List<string> Currencies { get; set; }

        public int CustomerId { get; set; }
    }
}
