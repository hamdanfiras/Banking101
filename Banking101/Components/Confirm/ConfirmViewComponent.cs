using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Components.Confirm
{
    public class ConfirmViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string message, string action, string controller, string submitButtonTitle, object values)
        {
            // The change to do more logic
            var model = new ConfirmComponentViewModel()
            {
                Action = action,
                Message = message,
                Controller = controller,
                SubmitButtonTitle = submitButtonTitle,
                Values = values
            };
            return View(model);
        }
    }
}
