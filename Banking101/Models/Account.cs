using System.ComponentModel.DataAnnotations;

namespace Banking101.Models
{
    public class Account
    {
        public int Id { get; set; }
       
        [MaxLength(3)]
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public Customer Customer { get; set; }
    }
}
