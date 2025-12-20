using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;

namespace CampusActivityHub.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            string logPath = Path.Combine(_env.ContentRootPath, "error_log.txt");
            string errorMessage = $"[{DateTime.Now}] ERROR: {context.Exception.Message}\nSTACK TRACE: {context.Exception.StackTrace}\n\n";
            
            File.AppendAllText(logPath, errorMessage);

            context.Result = new ViewResult { ViewName = "Error" };
            context.ExceptionHandled = true;
        }
    }
}