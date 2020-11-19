using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Display(Name = "Full Name")]
        [Required] 
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        
        public string Email { get; set; }

        public string Country { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public virtual List<Account> Accounts { get; set; }
    }



}
