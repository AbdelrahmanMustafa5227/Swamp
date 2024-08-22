using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Filters
{
    public class Filter1 : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Executed After Action");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Executed During Action");
        }
    }
}
