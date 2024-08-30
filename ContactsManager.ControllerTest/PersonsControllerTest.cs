using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Entities;
using ServiceContracts.DTO;
using CRUDE.Controllers;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace TestUnits
{
    public class PersonsControllerTest
    {
        private readonly IPersonGetterService _personService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PersonsController> _logger;

        private readonly Mock<ICountryService> _countryServiceMock;
        private readonly Mock<IPersonGetterService> _personsServicemock;
        private readonly IFixture _fixture;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        //solid principles
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonUpdaterServices _personUpdaterServices;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonSorterService _personSorterService;

        private readonly Mock<ICountryService> _countriesServiceMock;
        private readonly Mock<IPersonGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonUpdaterServices> _personsUpdaterServiceMock;
        private readonly Mock<IPersonSorterService> _personsSorterServiceMock;
        private readonly Mock<IPersonDeleterService> _personsDeleterServiceMock;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _countriesServiceMock = new Mock<ICountryService>();
            _personsGetterServiceMock = new Mock<IPersonGetterService>();
            _personsUpdaterServiceMock = new Mock<IPersonUpdaterServices>();
            _personsSorterServiceMock = new Mock<IPersonSorterService>();
            _personsAdderServiceMock = new Mock<IPersonAdderService>();
            _personsDeleterServiceMock= new Mock<IPersonDeleterService>();


            
            _loggerMock=new Mock<ILogger<PersonsController>>();
            _countryService=_countryServiceMock.Object;
            _personGetterService =_personsGetterServiceMock.Object;
            _personAdderService = _personsAdderServiceMock.Object;
            _personGetterService = _personsGetterServiceMock.Object;
            _personUpdaterServices = _personsUpdaterServiceMock.Object;
            _personDeleterService = _personsDeleterServiceMock.Object;
            _personSorterService= _personsSorterServiceMock.Object;

            _logger = _loggerMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_shouldReturnIndexViewWithPErsonList()
        {
            List<PersonResponse> Persons_resp_list = _fixture.Create<List<PersonResponse>>();

            PersonsController personController=new PersonsController
                (_personGetterService, _countryService, _logger, _personSorterService, _personDeleterService,_personAdderService,
                _personUpdaterServices);

            _personsGetterServiceMock.Setup(temp=>temp.GetFilteredPersons(It.IsAny<string>(),It.IsAny<string>() )).ReturnsAsync(Persons_resp_list);

            _personsSorterServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>() ,It.IsAny<string>(),It.IsAny<SortOrderOptions>())).ReturnsAsync(Persons_resp_list);

            //act
            IActionResult result=await  personController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //assert
            ViewResult viewResult=   Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().Be(Persons_resp_list);
        }

        #endregion

        #region  Create
        [Fact]
        public async void Create_IfModelErrors_ToReturnCreateView()
        {
            PersonAddRequest person_ad_request = _fixture.Create<PersonAddRequest>();
            PersonResponse person_resp = _fixture.Create<PersonResponse>();
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countryServiceMock.Setup(tem => tem.GetAllCountryList()).ReturnsAsync(countries);
            _personsAdderServiceMock.Setup(tem => tem.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_resp);

            PersonsController personController = new PersonsController(_personGetterService, _countryService, _logger, _personSorterService, _personDeleterService, _personAdderService,
                _personUpdaterServices);
            //we wantedly adding error inbelow line cause if error is there means only we have to get create view again from method
            personController.ModelState.AddModelError("PersonName", "Person name can't be blank");

            //act
            IActionResult result = await personController.Create(person_ad_request);

            //assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();

            viewResult.ViewData.Model.Should().Be(person_ad_request);
         }

        [Fact]
        public async void Create_IfNoModelErrors_ToRedirecttoIndexView()
        {
            PersonAddRequest person_ad_request = _fixture.Create<PersonAddRequest>();
            PersonResponse person_resp = _fixture.Create<PersonResponse>();
            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countryServiceMock.Setup(tem => tem.GetAllCountryList()).ReturnsAsync(countries);
            _personsAdderServiceMock.Setup(tem => tem.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_resp);

            PersonsController personController = new PersonsController(_personGetterService, _countryService, _logger, _personSorterService, _personDeleterService, _personAdderService,
                _personUpdaterServices);
            //we wantedly adding removed error inbelow line cause if error is there means only we have to get index view  from method means redirection
            // personController.ModelState.AddModelError("PersonName", "Person name cannot blank idiot");

            //act
            IActionResult result = await personController.Create(person_ad_request);

            //assert
             RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion


    }
}
