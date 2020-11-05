using Microsoft.EntityFrameworkCore;

namespace Banking101.Models
{
    public class BankingDB : DbContext
    {

        public BankingDB(DbContextOptions<BankingDB> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
