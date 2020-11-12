using Banking101.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Services
{
    public class CurrencyCalculator
    {
        private readonly BankingDB bankingDB;

        public CurrencyCalculator(BankingDB bankingDB)
        {
            this.bankingDB = bankingDB;
        }
        public decimal Calculate(string fromCurrency, string toCurrency, decimal amount)
        {
            // in real life i will be calling the db

            if (fromCurrency == "USD")
            {
                return amount * 1515;
            }
            else if (fromCurrency == "LBP")
            {
                return amount / 1515;
            }

            return amount;
        }
    }

     
}
