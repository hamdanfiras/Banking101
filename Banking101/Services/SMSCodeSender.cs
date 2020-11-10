using Banking101.Models;
using System;
using System.Threading.Tasks;

namespace Banking101
{
    public class SMSCodeSender : ICodeSender
    {
        public Task SendCode(Customer customer, string code)
        {
            throw new NotImplementedException();
        }
    }
}
