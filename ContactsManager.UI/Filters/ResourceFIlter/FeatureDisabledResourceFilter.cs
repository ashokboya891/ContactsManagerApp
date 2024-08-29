using CRUDE.Filters.ResultsFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDE.Filters.ResourceFIlter
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool _isDisabled;
        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled=true)
        {
            _logger = logger;
            _isDisabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation($"{nameof(FeatureDisabledResourceFilter)}.{(nameof(OnResourceExecutionAsync))} before exuting");
            if(_isDisabled)
            {
                // context.Result = new NotFoundResult(); //404--notfound 
                context.Result = new StatusCodeResult(501); //501-not implemented


            }
            else
            {

            await next();
            }
            _logger.LogInformation($"{nameof(FeatureDisabledResourceFilter)}.{(nameof(OnResourceExecutionAsync))} after exuting");

        }
    }
}
