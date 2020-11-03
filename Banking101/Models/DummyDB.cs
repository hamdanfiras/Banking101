using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Models
{
    public static class DummyDB
    {
        public static Account GetAccount(int id)
        {
            return new Account
            {
                Id = id,
                Amount = (new Random()).Next(1000, 10000),
                Currency = DateTime.Now.Ticks % 2 == 0 ? "USD" : "LBP",
                Customer = new Customer
                {
                    Email = "abc@gmail.com",
                    FullName = "Roger Federer", Id = 2
                }
            };
        }

        // Never use static variables for storing temporary application cache. Use Cache or Application state for this.
        private static List<Account> _tempAccounts;
        public static List<Account> GetAccounts(int customerId)
        {
            if (_tempAccounts == null)
            {
                Random rnd = new Random();
                _tempAccounts = new List<Account>();
                for (int i = 0; i < rnd.Next(30, 50); i++)
                {
                    _tempAccounts.Add(GetAccount(rnd.Next(1000, 999999)));
                }

            }
            return _tempAccounts;
        }

    }
}
