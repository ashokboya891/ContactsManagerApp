using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using Rotativa.AspNetCore;
using CRUDE.Filters.ActionFilters;
using CRUDE.Filters.ResultsFilters;
using CRUDE.Filters.ResourceFIlter;
using CRUDE.Filters.AuthorizationFilter;
using CRUDE.Filters.ExceptionsFilter;
using CRUDE.Filters.Overridefilters;
namespace CRUDE.Controllers
{
    [Route("[controller]")]
    [ResponseHeaderActionFilterFactory("Controller-Key", "Controller-Value", 3)]

    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Controller-Key", "Controller-Value" ,3},Order =3)]  //after implementing iorderedfilter below line commented we are pasing order value as arguments to filter
  //  [TypeFilter(typeof(HandleExceptionFilter))]
    /*[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "Controller-Key", "Controller-Value" },Order =2)]*/ //class level filter
    //order of filter execution depends on lowest to highest so here global->actionmethod-->controller cause 0-->1-->2 we can got with zero aswell it will consider precidence to exceute beofore zero
    public class PersonsController : Controller
    {
        //private fileds
//private readonly IPersonGetterService _personService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonsController> _logger;

        //solid principles
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonUpdaterServices _personUpdaterServices;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonSorterService _personSorterService;




        public PersonsController(IPersonGetterService personService, ICountryService countryService, 
            ILogger<PersonsController> logger,IPersonSorterService personSorterService,IPersonDeleterService personDeleterService,IPersonAdderService personAdderService,IPersonUpdaterServices personUpdaterServices)
        {
            this._countryService = countryService;
            _logger = logger;
            this._personGetterService = personService;
            this._personAdderService    = personAdderService;
            this._personUpdaterServices = personUpdaterServices;
            this._personDeleterService = personDeleterService;
            this._personSorterService= personSorterService;
        }
       
        
        
        //port/index persons is for whole controller and / will direclty take you to so we hahve remove / infront of index85
        [Route("[action]")]
        [Route("/")]
        [SkipFilter]  //used to skip any filter to not implement on action method
        [ServiceFilter(typeof(PersonsListActionFilter),Order =4)]
        //[TypeFilter(typeof(ResponseHeaderActionFilter),Arguments =new object[] {"Index-Key","Index-Value",1},Order =1)] //method level filter
        //[ResponseHeaderActionFilter("Index-Key", "Index-Value", 1)]
        [ResponseHeaderActionFilterFactory("Index-Key", "Index-Value", 1)]

        [TypeFilter(typeof(PersonsListResultFilter))]

        public async Task<IActionResult> Index(string? searchBy,string searchString,string sortBy=nameof(PersonResponse.PersonName)
            ,SortOrderOptions sortOrder=SortOrderOptions.ASC)
        {



            _logger.LogInformation("Index action method of prsons controllers");
            _logger.LogDebug($"searchBy:{searchBy} searchstring{ searchString}, sortby: {sortBy},sortorder:{sortOrder}");
            //belo code commented after adding filter for index
            //searching
            //ViewBag.SearchFields = new Dictionary<string, string>()
            //{
            //    { nameof(PersonResponse.PersonName),"Person Name"},
            //    { nameof(PersonResponse.Email),"Email"},
            //    { nameof(PersonResponse.DateOfBirth),"DateOfBirth"},
            //    { nameof(PersonResponse.Gender),"Gender"},
            //    { nameof(PersonResponse.CountryId),"Country"},
            //    { nameof(PersonResponse.Address),"Address"},

            //};
            //List<PersonResponse> resp = _personService.GetAllPersons();
            List<PersonResponse> resp =await _personGetterService.GetFilteredPersons(searchBy,searchString);
            //belo current named variables are getting assigned in actionfilter itlsef 
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.SearchString=searchString;  
            //sorting
           List<PersonResponse> sortedpersons=await _personSorterService.GetSortedPersons(resp,sortBy,sortOrder); 
            //ViewBag.CurrentSortBy=sortBy;
            //ViewBag.CurentSortOrder=sortOrder.ToString();
            return View(sortedpersons);
        }
        //exceutes when the user click on create persons hyperlink
        //(while openinig  create view))
        //URL:-persons/create persons is for whole controller and exact method will invoke that name which we mention

        [Route("[action]")]
        [HttpGet]
        //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "create-Key", "create-Value",4 })]
        [ResponseHeaderActionFilterFactory("create-key","create-value",4)]
        //[ResponseHeaderActionFilter("create-key","create-value",4)]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countryList = await _countryService.GetAllCountryList();
            ViewBag.Countries = countryList.Select(temp=>new SelectListItem()
            {
                //Selected = true,
                Text = temp.CountryName,
                Value = temp.CountryId.ToString()

            });
            //new SelectListItem()
            //{
            //    Selected=true,
            //    Text="Harsha",
            //    Value="1"
            //};
            return View();
        }
        //URL:-persons/create persons is for whole controller and exact method will invoke that name which we mention

        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(PersonCreteAndEditPostActionFilter))]

        // [TypeFilter(typeof(FeatureDisabledResourceFilter),Arguments =new object[] {true})]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            //cmnted after adding PersonCreteAndEditPostActionFilter it is working for below code
            //if(!ModelState.IsValid)
            //{
            //    List<CountryResponse> countryList =await _countryService.GetAllCountryList();
            //    ViewBag.Countries = countryList.Select(temp => new SelectListItem()
            //    {
            //        Text = temp.CountryName,
            //        Value = temp.CountryId.ToString()

            //    });
            //    ViewBag.Errors  =  ModelState.Values.SelectMany(v => v.Errors).Select(e=>e.ErrorMessage).ToList();
            //    return View();
            //}
            //call the service method
            PersonResponse resp =await _personAdderService.AddPerson(personRequest);
            //navigate to index()  it will redirect to index 
            return RedirectToAction("Index","Persons");
        }

        [HttpGet]
        [Route("[action]/{PersonID}")] //EG:/persons/edit/1
        [TypeFilter(typeof(TokenResulltFilter))]

        public async Task< IActionResult> Edit(Guid PersonID)
        {
           PersonResponse? personResponse=await _personGetterService.GetPersonByPersonId(PersonID);
            if(personResponse==null)
            {
                return RedirectToAction("Index");


            }


            PersonUpdateRequest personupdate = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countries = await _countryService.GetAllCountryList();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() ,Selected=true});
         
            return View(personupdate);
        }
        [HttpPost]
        [Route("[action]")] //EG:/persons/edit/1
        [TypeFilter(typeof(PersonCreteAndEditPostActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        [TypeFilter(typeof(PersonAlwaysRunResultFilter))]


        public async Task< IActionResult> Edit(PersonUpdateRequest personRequest)
         {
            PersonResponse? resp=await  _personUpdaterServices.UpdatePerson(personRequest);
            if(resp==null)
            {
                return RedirectToAction("Index");
            }
           // personRequest.PersonID = Guid.NewGuid();

            PersonResponse obj = await  _personUpdaterServices.UpdatePerson(personRequest);
                return RedirectToAction("Index");
            //cmnted after adding PersonCreteAndEditPostActionFilter it is working for below code
            //if(ModelState.IsValid)
            //{
            //PersonResponse obj = await _personService.UpdatePerson(personRequest);
            //return RedirectToAction("Index");
            //}
            //else
            //{
            //    List<CountryResponse> countries =await _countryService.GetAllCountryList();
            //    ViewBag.Countries = countries.Select(temp =>
            //    new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });

            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    return View(resp.ToPersonUpdateRequest());
            //}
            //return View();
        }
        [HttpGet]
        [Route("[action]/{PersonID}")] //EG:/persons/Delete/1
        public async Task<IActionResult> Delete(Guid PersonID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");

            }


            //PersonResponse personupdate = personResponse.();
            List<CountryResponse> countries = await _countryService.GetAllCountryList();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString(), Selected = true });

            return View(personResponse);
        }
        [HttpPost]
        [Route("[action]")] //EG:/persons/Delete/1
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? resp1 =await _personGetterService.GetPersonByPersonId(personUpdateRequest.PersonID);
            if (resp1 == null)
            {
                return RedirectToAction("Index");
            }
            else
            {

            bool? resp =await _personDeleterService.DeletePerson(personUpdateRequest.PersonID);
             return RedirectToAction("Index");
            }
         
            //return View(resp);
             
        }
        [Route("PersonsPdf")]
        public async Task<IActionResult> PersonsPdf()
        {
            List<PersonResponse> person =await _personGetterService.GetAllPersons();
            //return new viewAs
            //return new ViewAsPdf("PersonsPdf", person, ViewData)
            //{
            //    PageMargins = new Rotativa.AspNetCore.Options.Margins()
            //    {
            //        Top = 20,
            //        Bottom = 20,
            //        Left = 20,
            //        Right = 20

            //    },
            //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape


            // };
            return new ViewAsPdf("PersonsPdf", person, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 20,
                    Right = 20

                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        [Route("PersonsCsv")]
        public async Task<IActionResult> PersonsCsv()
        {
            MemoryStream memoryStream = await _personGetterService.GetPersonsCSV();
            return File(memoryStream,"application/octet-stream","persons.csv");

        }
        [Route("PersonsExc")]
        public async Task<IActionResult> PersonsExc()
        {
             MemoryStream memoryStream = await _personGetterService.GetPersonsExc();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Persons.xlsx");

        }
    }
}
