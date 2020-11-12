using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Components.Confirm
{
    public class ConfirmComponentViewModel
    {
        public string Message { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string SubmitButtonTitle { get; set; }
        public object Values { get; set; }
    }
}
