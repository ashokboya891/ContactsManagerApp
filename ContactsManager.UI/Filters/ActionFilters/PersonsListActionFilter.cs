using CRUDE.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDE.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            this._logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //logic to do:after OnExceuting below 
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter),nameof(OnActionExecuted));
            PersonsController personsController=(PersonsController)context.Controller;
            IDictionary<String,object?>? paramters=(IDictionary<string,object?>?) context.HttpContext.Items["arguments"];
            if(paramters!=null)
            {
                if(paramters.ContainsKey("searchBy"))
                {

                 personsController.ViewData["CurrentSearchBy"] = Convert.ToString( paramters["searchBy"]);
                }
                if (paramters.ContainsKey("searchString"))
                {

                    personsController.ViewData["SearchString"] =Convert.ToString( paramters["searchString"]);
                }
                if (paramters.ContainsKey("sortBy"))
                {

                    personsController.ViewData["CurrentSortBy"] = Convert.ToString( paramters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);

                }
                if (paramters.ContainsKey("sortOrder"))
                {

                    personsController.ViewData["CurentSortOrder"] = Convert.ToString(paramters["sortOrder"]) ;
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(SortOrderOptions.ASC);

                }

                personsController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                { nameof(PersonResponse.PersonName),"Person Name"},
                { nameof(PersonResponse.Email),"Email"},
                { nameof(PersonResponse.DateOfBirth),"DateOfBirth"},
                { nameof(PersonResponse.Gender),"Gender"},
                { nameof(PersonResponse.CountryId),"Country"},
                { nameof(PersonResponse.Address),"Address"},

                 };
            }
            
        }

        //string searchBy,string searchString,string sortBy = nameof(PersonResponse.PersonName)
        //    , SortOrderOptions sortOrder=SortOrderOptions.ASC)
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //we cannot access controller parameters directly in OnExcuted method above so we are storing those paramters in globalized dictinary which is below items
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter),nameof(OnActionExecuting));

            //logic to do:before execuition 
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if(!string.IsNullOrEmpty(searchBy))
                {
                    var searchOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.CountryId),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.Age),
                        nameof(PersonResponse.Address),
                        nameof(PersonResponse.ReceiveLetters),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth)
                    };
                    if (searchOptions.Any(temp => temp == searchBy)==false)
                    {
                        _logger.LogInformation("actual searchyBy:{searchBy}", searchBy);
                        context.ActionArguments["searchBy"]=nameof(PersonResponse.PersonName);
                        _logger.LogInformation("after updating using filter {searchBy}:", context.ActionArguments["searchBy"]);


                    }
                }
                
            }

        }
    }
}
