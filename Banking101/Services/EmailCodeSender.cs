using Banking101.Models;
using System;
using System.Threading.Tasks;

namespace Banking101
{
    public class EmailCodeSender : ICodeSender
    {
        public Task SendCode(Customer customer, string code)
        {
            // real code to send emails
            throw new NotImplementedException();
        }
    }
}
