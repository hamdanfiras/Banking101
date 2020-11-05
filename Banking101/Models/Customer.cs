using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public List<Account> Accounts { get; set; }
    }
}
