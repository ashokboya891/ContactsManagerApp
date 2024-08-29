using Serilog.Extensions.Hosting;

namespace CRUDE.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly DiagnosticContext _diagnosticContext;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, DiagnosticContext diagnosticContext)
        {
            _next = next;
            this._diagnosticContext = diagnosticContext;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext httpcontext)
        {
            try
            {
                await _next(httpcontext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType}{ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType}{ExceptionMessage}", ex.GetType().ToString(), ex.Message);

                }
                   // httpcontext.Response.StatusCode = 500;
                    //await httpcontext.Response.WriteAsync("Error occured");
                   throw;
                
            }
        }
    }
}
