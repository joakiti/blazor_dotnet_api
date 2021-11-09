using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlatformDemo.ActionFilter
{
    public class Tickets_EnsureDateInFuture : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var ticket = context.ActionArguments["ticket"] as Ticket;

            if (ticket != null && ticket.Title == "forbidden title")
            {
                context.ModelState.AddModelError("Title", "Title contains forbidden title");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
