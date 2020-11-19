using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Banking101.Models;
using Castle.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Banking101.Controllers
{
    public class CustomersController : Controller
    {
        private readonly BankingDB _context;

        // basic configuration access
        private readonly IConfiguration configuration;

        // ioptions configuration access
        private readonly SMSServiceOptions _smsOptions;

        public CustomersController(BankingDB context, IOptions<SMSServiceOptions> smsOptions, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
            _smsOptions = smsOptions.Value;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();
            IQueryable<Customer> customers2 = from c in _context.Customers select c;
            //var customers =  _context.Customers.FromSqlRaw("select * from customers").ToList();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // Eager loading
            Customer customer = await _context.Customers
                .Include(x => x.Accounts)
                .FirstOrDefaultAsync(m => m.Id == id);


            // explicit loading
            Customer customer2 = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);

            _context.Entry(customer2)
                .Collection(c => c.Accounts)
                .Load();

            // lazy loading
            //Customer customer3 = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            //var accounts = customer.Accounts;



            //customer.Accounts (count 5)

            //        var city = _context.Cities
            //.Single(c => c.CityId == 1);

            //        _context.Entry(city)
            //            .Collection(c => c.People)
            //            .Load();


            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FullName,Email,CreateDate")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
