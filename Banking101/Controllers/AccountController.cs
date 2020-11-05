using Banking101.Models;
using Banking101.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Banking101.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index(int? id)
        {
            // simulate calling database 

            if (id == null)
            {
                return BadRequest();
            }

            Account account = null; //DummyDB.GetAccount(id.Value);

            return View(account);
        }

        public ActionResult List(int customerId, int page = 0, int rowsPerPage = 5)
        {
            List<Account> accounts = new List<Account>();// DummyDB.GetAccounts(customerId);

            var vm = new AccountListVM();
            vm.CustomerId = customerId;
            vm.Accounts = new PagedList<Account>
            {
                Page = page,
                RowsPerPage = rowsPerPage,
                Data = accounts.Skip(page * rowsPerPage).Take(rowsPerPage).ToList(),
                TotalCount = accounts.Count
            };
            vm.Currencies = new List<string> { "USD", "LBP" };
            return View(vm);
        }
    }
}
