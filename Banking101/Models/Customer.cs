using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public List<Account> Accounts { get; set; }
    }

    public class BankDB : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}
