using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking101.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking101.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            Customer c = new Customer();
            c.Accounts = new List<Account>();
            
            // save to database
            return View();
        }

        // https://facebook.com/hamdanfiras
        // youtube.com/video/2736723j
    }
}
