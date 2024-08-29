using CRUDE.Filters.Overridefilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDE.Filters.ResultsFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }
    }
}
