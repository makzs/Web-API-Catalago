using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        //executa depois do metodo action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("-- Executando -> OnActionExecuted");
            _logger.LogInformation("-----------------------------------------");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString}");
            _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("-----------------------------------------");
        }

        //executa antes do metodo action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("-- Executando -> OnActionExecuting --");
            _logger.LogInformation("-----------------------------------------");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("-----------------------------------------");
        }
    }
}
