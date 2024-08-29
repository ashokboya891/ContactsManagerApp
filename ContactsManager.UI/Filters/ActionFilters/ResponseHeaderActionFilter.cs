using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDE.Filters.ActionFilters
{
    //controller-->filterfactory-->filter

    public class ResponseHeaderActionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private readonly string Key;
        private readonly string Value;
        public int Order { get; }

        public bool IsReusable => false;
        public ResponseHeaderActionFilterFactoryAttribute(string key,string value,int order)
        {
            this.Key = key;
            this.Value = value;
            this.Order = order;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //return filter object
            //var filter= new ResponseHeaderActionFilter(Key,Value,Order);
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();

            filter.Order = Order;
            filter.Key = Key;
            filter.Value = Value;
            return filter;
        }
    }
    //public class ResponseHeaderActionFilter : IActionFilter,IOrderedFilter
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    //public class ResponseHeaderActionFilter : ActionFilterAttribute

    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;

        //private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public  string Key { get; set; }
        public  string Value { get; set; }
        public int Order { get; set; }


        //public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger,string key,string value,int order)
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)

        {
            this._logger = logger;
            //    this.Key = key;
            //    this.Value = value;
            //    this.Order = order;
        }


        //below two methods m=cmntd after implementing iasyncactionfilter

        //afyer
        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName} exceuted", (nameof(ResponseHeaderActionFilter)), (nameof(OnActionExecuted)));
        //    context.HttpContext.Response.Headers[Key] = Value;
        //}
        //before

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName} exceuted", (nameof(ResponseHeaderActionFilter)), (nameof(OnActionExecuting)));
        //}

        public  async  Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //onExcecuting code

            _logger.LogInformation("{FilterName}.{MethodName} Method-before", (nameof(ResponseHeaderActionFilter)), (nameof(OnActionExecutionAsync)));
            

            await next();  //delegate  calls subsequest filter or action method otherwise leads to shortcircuit or cte
           
           //onExcutedcode
            _logger.LogInformation("{FilterName}.{MethodName} Method-After", (nameof(ResponseHeaderActionFilter)), (nameof(OnActionExecutionAsync)));
              context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
