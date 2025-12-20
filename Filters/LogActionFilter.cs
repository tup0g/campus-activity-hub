using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace CampusActivityHub.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private Stopwatch _stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var time = _stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"[LOG] User accessed {controller}/{action} - Executed in {time} ms");
        }
    }
}