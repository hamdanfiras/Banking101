using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Filters
{
    public class SendVersionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("VERSION", "1.2.0");
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
         
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
      
        }
    }
}
