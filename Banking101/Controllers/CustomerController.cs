using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Banking101.Filters;
using Banking101.Models;
using Banking101.Services;
using Banking101.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking101.Controllers
{

    [Route("Customer")]
    public class CustomerController : Controller
    {
        private readonly BankingDB _db;
        private readonly ICodeSender _codeSender;
        private readonly IBulkCodeSender bulkCodeSender;
        private readonly CurrencyCalculator currencyCalculator;
        private readonly IHostingEnvironment hostingEnvironment;

        public CustomerController(BankingDB db, ICodeSender codeSender, IBulkCodeSender bulkCodeSender, CurrencyCalculator currencyCalculator, IHostingEnvironment hostingEnvironment)
        {
            this._db = db;
            this._codeSender = codeSender;
            this.bulkCodeSender = bulkCodeSender;
            this.currencyCalculator = currencyCalculator;
            this.hostingEnvironment = hostingEnvironment;
        }

        [SendVersion]
        [Route("Index")]
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

        [HttpPost]
        [Route("GenerateDummyCustomers")]
        public ActionResult GenerateDummyCustomers()
        {
            // "c:\test\randomnames.txt

            string[] lines = System.IO.File.ReadAllLines(@"c:\test\randomnames.txt");

            //List<Customer> customers = new List<Customer>();
            //foreach (var line in lines)
            //{
            //    var fullName = line.Trim();
            //    if (!string.IsNullOrWhiteSpace(fullName))
            //    {
            //        customers.Add(new Customer
            //        {
            //            Id = Guid.NewGuid(),
            //            Email = $"{fullName.Split(' ')[0]}@gmail.com",
            //            FullName = fullName
            //        }); ;
            //    }
            //}

            var customers = lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => new Customer
                {
                    Id = Guid.NewGuid(),
                    Email = $"{l.Trim().Split(' ')[0]}@gmail.com".ToLower(),
                    FullName = l.Trim()
                })
                .ToList();

            _db.Customers.AddRange(customers);
            _db.SaveChanges();

            return RedirectToAction("Index");
            //return RedirectToRoute("/Customer/Index");
        }

        [HttpPost]
        public ActionResult CleanUp()
        {
            var customers = _db.Customers
                .Include(x => x.Accounts)
                .ToList();

            var toDelete = customers.Where(x => x.Accounts.Count == 0).ToList();

            _db.Customers.RemoveRange(toDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
            //return StatusCode(404);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            Customer customer = _db.Customers.FirstOrDefault(x => x.Id == id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult ConfirmDelete(Guid id)
        {
            var customer = _db.Customers.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SendCode(Guid id)
        {
            // we send the code here

            string code = (new Random()).Next(100000, 999999).ToString();
            var customer = _db.Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                _codeSender.SendCode(customer, code);
            }

            return RedirectToAction("Index");

        }

        public async Task<ActionResult> SendBulkCode()
        {
            // we send the code here
            var rnd = new Random();
            var customerCodes = _db.Customers.ToDictionary(x => x, x => rnd.Next(100000, 999999).ToString());

            await bulkCodeSender.SendBulkCode(customerCodes);
            return RedirectToAction("Index");
        }

        [Route("TestParams/{xyz}/{abc}")]
        public async Task<ActionResult> TestParams([FromRoute] string xyz, [FromRoute] string abc)
        {
            return Content(xyz + " / " + abc, "text/plain");
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customer customer)
        {
            // after validation
            _db.Customers.Add(customer);
            _db.SaveChanges();

            //_db.Entry(customer.Accounts[0]).State = EntityState.;

            return RedirectToAction("Index");
        }

        //[Route("TestParams")]
        //public async Task<ActionResult> TestParams()
        //{
        //    return Content("hello world");
        //}
    }
}
