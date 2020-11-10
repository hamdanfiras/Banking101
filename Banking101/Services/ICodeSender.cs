using Banking101.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101
{
    public interface ICodeSender
    {
        Task SendCode(Customer customer, string code);
    }
}
