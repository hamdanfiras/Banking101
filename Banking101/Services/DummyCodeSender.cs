using Banking101.Models;
using System;
using System.Threading.Tasks;

namespace Banking101
{
    public class DummyCodeSender : ICodeSender
    {
        public DummyCodeSender()
        {

        }

        public  Task SendCode(Customer customer, string code)
        {
            Console.WriteLine($"Send code: {code} to customer {customer.FullName}");
            return Task.FromResult(0);
        }
    }
}
