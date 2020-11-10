using Banking101.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking101
{
    public interface IBulkCodeSender
    {
        Task SendBulkCode(Dictionary<Customer, string> customerCode);
    }

    public class BulkCodeSender : IBulkCodeSender
    {
        private readonly ICodeSender codeSender;

        public BulkCodeSender(ICodeSender codeSender)
        {
            this.codeSender = codeSender;
        }

        public async Task SendBulkCode(Dictionary<Customer, string> customerCodes)
        {
            foreach (var customer in customerCodes.Keys)
            {
                var code = customerCodes[customer];
                await codeSender.SendCode(customer, code);
            }
        }
    }
}
