using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101
{
    public class SMSServiceOptions
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 8080;
        public string From { get; set; }
    }
}
