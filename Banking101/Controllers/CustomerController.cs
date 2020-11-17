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
        private readonly IWebHostEnvironment hostingEnvironment;

        public CustomerController(BankingDB db, ICodeSender codeSender, IBulkCodeSender bulkCodeSender, CurrencyCalculator currencyCalculator, IWebHostEnvironment hostingEnvironment)
        {
            this._db = db;
            this._codeSender = codeSender;
            this.bulkCodeSender = bulkCodeSender;
            this.currencyCalculator = currencyCalculator;
            this.hostingEnvironment = hostingEnvironment;
        }

        [SendVersion]
        [Route("Index")]
        public ActionResult Index(Guid? selectedCustomerId, string country = null, int page = 0, int rowsPerPage = 5)
        {
            int count = _db.Customers.Count();

            List<Customer> customers = _db.Customers
                .Where(x => country == null || x.Country == country)
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
                SelectedCustomer = selectedCustomer,
                SelectedCountry = country,
                AllCountries = Countries.GetCountries()
            };

            return View(vm);
        }

        [Route("Details")]
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
        [Route("CleanUp")]
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
        [Route("Delete")]
        public ActionResult Delete(Guid id)
        {
            Customer customer = _db.Customers.FirstOrDefault(x => x.Id == id);
            return View(customer);
        }

        [HttpPost]
        [Route("ConfirmDelete")]
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

        [Route("SendCode")]
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

        [Route("SendBulkCode")]
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


        [Route("Add")]
        [HttpGet]
        public async Task<ActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Add(Customer customer)
        {
            // after validation


            if (customer == null)
            {
                ModelState.AddModelError("EmptyCustomer", "The customer is empty");
                return View(customer);
            }

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            if (customer.Id == default)
            {
                customer.Id = Guid.NewGuid();
            }

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult Edit(Guid id)
        {
            var customer = _db.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public ActionResult Edit([FromRoute] Guid id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var dbCustomer = _db.Customers.FirstOrDefault(x => x.Id == id);
            if (dbCustomer == null)
            {
                return RedirectToAction("Index");
            }

            dbCustomer.FullName = customer.FullName;
            dbCustomer.Email = customer.Email;

            _db.SaveChanges();


            return RedirectToAction("Index");
        }

    }
}
