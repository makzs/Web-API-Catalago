using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu uma execção não tratada: Status Code: 500");

            context.Result = new ObjectResult("Ocorreu um problema ao tratar sua solicitação: Status 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
