using CRUDE.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDE.Filters.ActionFilters
{
    public class PersonCreteAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonCreteAndEditPostActionFilter> _logger;
            private readonly ICountryService _countryService;
        public PersonCreteAndEditPostActionFilter(ICountryService countryService, ILogger<PersonCreteAndEditPostActionFilter> logger)
        {
            this._countryService = countryService;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {
                //to do before logic
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countryList = await _countryService.GetAllCountryList();
                    personsController.ViewBag.Countries = countryList.Select(temp => new SelectListItem()
                    {
                        //Selected = true,
                        Text = temp.CountryName,
                        Value = temp.CountryId.ToString()

                    });
                    personsController.ViewBag.Errors = personsController. ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result= personsController.View(personRequest);    //short circuits or skips the subsequent action filters
                }
                else
                {
                    await next();  // invokes the subsequest filter or actionmethod 
                }

            }
            else
            {
                await next();  // invokes the subsequest filter or actionmethod 

            }


            //to do: after logic
            _logger.LogInformation("In after logiv of PersonCreteAndEditPostActionFilter filter");
        }
    }
}
