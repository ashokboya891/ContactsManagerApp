using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDE.Filters.ResultsFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation($"{nameof(PersonsListResultFilter)}.{(nameof(OnResultExecutionAsync))} before exuting");
            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:MM");
            await next(); //cals subsequest filter or iactionresult
            _logger.LogInformation($"{nameof(PersonsListResultFilter)}.{(nameof(OnResultExecutionAsync))} after exuting");
        }
    }
}
