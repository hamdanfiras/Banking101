using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Banking101.Models;
using Banking101.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking101.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankingDB _db;

        public CustomerController(BankingDB db)
        {
            this._db = db;
        }

        public ActionResult Index(Guid? selectedCustomerId, int page = 0, int rowsPerPage = 5)
        {
            int count = _db.Customers.Count();

            List<Customer> customers = _db.Customers
                .Include(c => c.Accounts)
                .Skip(page * rowsPerPage)
                .Take(rowsPerPage)
                .ToList();


            Customer selectedCustomer = null;
            if (selectedCustomerId != null)
            {
                selectedCustomer = _db.Customers.FirstOrDefault(x => x.Id == selectedCustomerId);
            }

            //// row grouping
            //List<IGrouping<string, Account>> accountsGroup = _db.Accounts.GroupBy(x => x.Currency).ToList();

            //foreach (var group in accountsGroup)
            //{
            //    var curency = group.Key;
            //    foreach (var account in group)
            //    {
            //        // listing the accounts in each currency
            //    }
            //}

            //// aggregate grouping
            //Dictionary<string, int> countByCurrency = _db.Accounts
            //     .GroupBy(x => x.Currency)
            //     .ToDictionary(x => x.Key, x => x.Count());



            var vm = new CustomerIndexVM
            {
                CustomersPageList = new PagedList<Customer>
                {
                    Page = page,
                    RowsPerPage = rowsPerPage,
                    TotalCount = count,
                    Data = customers
                },
                SelectedCustomer = selectedCustomer
            };

            return View(vm);
        }

        public ActionResult Details(Guid? id)
        {
            return View();
        }
    }
}
