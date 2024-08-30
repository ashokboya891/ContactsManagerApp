using Entities;

using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;
using Xunit.Sdk;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using System.Reflection.Metadata;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
namespace TestUnits
{
    public class PersonServiceTest
    {
        //private readonly IPersonGetterService _personService;
        private readonly IFixture _fixture;

        private ICountryService _countryService;

        private readonly Mock<IPersonRespository> _personRepositoryMock;
        private readonly IPersonRespository _personsRepository;
        //solid principles
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonUpdaterServices _personUpdaterServices;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonSorterService _personSorterService;

        private readonly ITestOutputHelper _testOutputHelper;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();    
            //var countriesIntialDate = new List<Country>() { };
            //var personsIntialDate = new List<Person>() { };

            //DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
            //    new DbContextOptionsBuilder<ApplicationDbContext>().Options
            //    );
            //ApplicationDbContext dbContext = dbContextMock.Object;
            //dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesIntialDate);
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, personsIntialDate);

            //this._countryService = new CountryService(null);
               // this._personService = new PersonService(null)
             
            this._testOutputHelper = testOutputHelper;
             
            _personRepositoryMock = new Mock<IPersonRespository>();
            _personsRepository = _personRepositoryMock.Object;


            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<ILogger<PersonGetterService>>();



            _personGetterService = new PersonGetterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personAdderService = new PersonAdderService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personDeleterService = new PersonDeleterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personSorterService = new PersonSorterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);

            _personSorterService = new PersonSorterService(_personsRepository, loggerMock.Object, diagnosticContextMock.Object);
        }
        //public PersonServiceTest(ITestOutputHelper testOutputHelper)
        //{
            //this._personService = personservice;
            //in EF migrations after using real db this lines changed
            //_countryService = new CountryService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
            //this._personService = new PersonService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options),_countryService);
            //this._countryService = new CountryService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
            //this._testOutputHelper = testOutputHelper;
        //}
        #region AddPerson

        //when we supply null value as personaddrequest it should throu argument null exception
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            Func<Task> action = async () =>
            {
                await _personAdderService.AddPerson(personAddRequest);

            };
            await action.Should().ThrowAsync<ArgumentNullException>();

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //   await _personService.AddPerson(personAddRequest);
            //});
        }

        //when we supply null value as personname it should throu argumentexception
        [Fact]
        public async Task AddPerson_PersonNameisNull()
        {
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(te => te.PersonName, null as string).Create();
            //new PersonAddRequest() { PersonName=null};

            Func<Task> action = async () =>
            {
                await _personAdderService.AddPerson(personAddRequest);

            };
            await action.Should().ThrowAsync<ArgumentException>();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //   await _personService.AddPerson(personAddRequest);
            //});
        }

        //when we supply proper   personname details it should insert the person into perosns list and it should return
        //an object of personresponse which included with the newly generated person id

        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With
                (temp => temp.Email, "sae@example.com").With(temp=>temp.PersonName,"king"). 
                Create();
            //    new PersonAddRequest() { 
            //    PersonName = "name..." ,
            //    Email = "ashok@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = Guid.NewGuid(),
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
                
            //};
            //act
           PersonResponse person_resp_from_add=await _personAdderService.AddPerson(personAddRequest);

            List<PersonResponse> person_resp_list=await _personGetterService.GetAllPersons();
            //assert

            person_resp_from_add.PersonId.Should().NotBe(Guid.Empty);
            //Assert.True(person_resp_from_add.PersonId!=Guid.Empty);
           person_resp_list.Should().Contain(person_resp_from_add);
           // Assert.Contains(person_resp_from_add,person_resp_list);
        }
        #endregion

        #region GetPersonByPersonID
        //if we supply null as person id it should return null as personrespons
        [Fact]
        public async Task GetPersonByPersonId()
        {
            //arrange
            Guid? personId = null;
            //act
            PersonResponse? person_resp_from_get= await _personGetterService.GetPersonByPersonId(personId);

            //assert
            person_resp_from_get.Should().BeNull();
            //Assert.Null(person_resp_from_get);

        }
        //if we supply a valid person id is shold return the valid person details as personresponse object 
        [Fact]
        public async Task GetPersonByPersonId_withPersonId()
        {
            CountryAddRequest? countryAddRequest = _fixture.Create<CountryAddRequest>();
            // new CountryAddRequest()
            //{
            //    CountryName = "Africa"
            //};
            CountryResponse country_resp=await _countryService.AddCountry(countryAddRequest);
            //act
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").Create();
            //    new PersonAddRequest() 
            //{
            //    PersonName = "person name..." ,
            //    Email = "ashok12@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = country_resp.CountryId,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),

            //};
            PersonResponse person_resp_add= await _personAdderService.AddPerson(personAddRequest);
            PersonResponse? person_resp_from_get =await _personGetterService.GetPersonByPersonId(person_resp_add.PersonId);

            //assert
            person_resp_from_get.Should().Be(person_resp_from_get);
            //Assert.Equal(person_resp_add,person_resp_from_get);


        }

        #endregion

        #region GetAllPersons
        //the getallpersons() shoiuld return empty list by default
        [Fact]
        public async Task GetAllPersons()
        {
            //act
            List<PersonResponse> persons_from_get=await _personGetterService.GetAllPersons();
            //assert
            persons_from_get.Should().BeEmpty();
            //Assert.Empty(persons_from_get);
        }

        //first we will add few persons and then when we call getallpersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            CountryAddRequest? countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "iceland"
            //};
            CountryAddRequest? countryAddRequest2 = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //    new CountryAddRequest()
            //{
            //    CountryName = "Antarcitca"
            //};
            CountryResponse resp1 =await _countryService.AddCountry(countryAddRequest1);
            CountryResponse resp2= await _countryService.AddCountry(countryAddRequest2);

            PersonAddRequest? personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").Create();
            PersonAddRequest? personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "king@gmail.com").Create();
            PersonAddRequest? personAddRequest3= _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "kangana@gmail.com").Create();


            //new PersonAddRequest()
            //{
            //    PersonName = "person name...",
            //    Email = "ashok12@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = resp1.CountryId,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),

            //};
            //PersonAddRequest? personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "kingmen",
            //    Email = "kings@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KDP",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-01-01"),

            //};
            //PersonAddRequest? personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "mounika",
            //    Email = "mounika@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KNlM",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-01-01"),

            //};
            //_personService.AddPerson(personAddRequest2);
            //_personService.AddPerson(personAddRequest1);
            //_personService.AddPerson(personAddRequest3);
            //above 3 lines code and below code acts same 
            List<PersonAddRequest> request_lst = new
            List<PersonAddRequest>() { personAddRequest1,
                    personAddRequest2,personAddRequest3
            };

            List<PersonResponse> person_resp_list_from_add = new List<PersonResponse>();
            foreach (var item in request_lst)
            {
                
                PersonResponse person_res= await _personAdderService.AddPerson(item);
                person_resp_list_from_add.Add(person_res);
            }
            //print
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_add in person_resp_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_add.ToString());
            }
            //act
            List<PersonResponse> persons_list_from_get=await _personGetterService.GetAllPersons();
            //print persons lists from get
            _testOutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_get.ToString());
            }
            //assert

            persons_list_from_get.Should().BeEquivalentTo(person_resp_list_from_add);
            //foreach (var item in person_resp_list_from_add)
            //{
            //    Assert.Contains(item, persons_list_from_get);
            //}
        }
        #endregion

        #region GetFiltered
        //if the search text is empty and seach by is "personame" it should return all persons
        [Fact]
        public  async Task GetFilteredPersons_Emptysearchtext()
        {
            CountryAddRequest? countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            //new CountryAddRequest()
            //{
            //    CountryName = "cleveland"
            //};
            CountryAddRequest? countryAddRequest2 = _fixture.Create<CountryAddRequest>();
            //new CountryAddRequest()
            //{
            //    CountryName = "arcitca"
            //};
            CountryResponse resp1 = await _countryService.AddCountry(countryAddRequest1);
            CountryResponse resp2 =await _countryService.AddCountry(countryAddRequest2);

            PersonAddRequest? personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "kangana@gmail.com").With(t=>t.PersonName,"king").Create();
            PersonAddRequest? personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "king@gmail.com").With(t=>t.PersonName,"king").Create();
            PersonAddRequest? personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "bing").Create();
            //new PersonAddRequest()
            //{
            //    PersonName = "ashokboya",
            //    Email = "ashok12@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = resp1.CountryId,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),

            //};
            //    new PersonAddRequest()
            //{
            //    PersonName = "kingmou",
            //    Email = "kings@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KDP",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-01-01"),

            //};
            //    new PersonAddRequest()
            //{
            //    PersonName = "mounika",
            //    Email = "mounika@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KNlM",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-01-01"),

            //};

            List<PersonAddRequest> request_lst = new
            List<PersonAddRequest>() { personAddRequest1,
                    personAddRequest2,personAddRequest3
            };

            List<PersonResponse> person_resp_list_from_add = new List<PersonResponse>();
            foreach (var item in request_lst)
            {

                PersonResponse person_res = await _personAdderService.AddPerson(item);
                person_resp_list_from_add.Add(person_res);
            }
            //print
            _testOutputHelper.WriteLine("Expected: ");

            foreach (PersonResponse person_response_add in person_resp_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_add.ToString());
            }
            


            //act
            List<PersonResponse> persons_list_from_search = await _personGetterService.GetFilteredPersons(nameof(Person.PersonName),"");
            //print persons lists from get
            _testOutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_get.ToString());
            }
            //assert
            persons_list_from_search.Should().BeEquivalentTo(person_resp_list_from_add);
            //foreach (var item in person_resp_list_from_add)
            //{
               
            //    Assert.Contains(item, persons_list_from_search);
            //}
        }

        //first we will add few perosns and then search it with personname with search string it hould returne matched results
        [Fact]
        public async Task  GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest? countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "cleveland"
            //};
            CountryAddRequest? countryAddRequest2 = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "arcitca"
            //};
            CountryResponse resp1 = await _countryService.AddCountry(countryAddRequest1);
            CountryResponse resp2 = await _countryService.AddCountry(countryAddRequest2);
            PersonAddRequest? personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "kangana@gmail.com").With(t => t.PersonName, "marayman").With(t=>t.CountryId,resp1.CountryId).Create();
            PersonAddRequest? personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "king@gmail.com").With(t => t.PersonName, "rahaman1").With(t => t.CountryId, resp1.CountryId).Create();
            PersonAddRequest? personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "king").With(t => t.CountryId, resp2.CountryId).Create();
            //new PersonAddRequest()
            //PersonAddRequest? personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "ashokboya",
            //    Email = "ashok12@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = resp1.CountryId,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),

            //};
            //PersonAddRequest? personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "kingmou",
            //    Email = "kings@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Other,
            //    Address = "KDP",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-01-01"),

            //};
            //PersonAddRequest? personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "mounika",
            //    Email = "mounika@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KNlM",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-01-01"),

            //};

            List<PersonAddRequest> request_lst = new
            List<PersonAddRequest>() { personAddRequest1,
                    personAddRequest2,personAddRequest3
            };

            List<PersonResponse> person_resp_list_from_add = new List<PersonResponse>();
            foreach (var item in request_lst)
            {

                PersonResponse person_res = await _personAdderService.AddPerson(item);
                person_resp_list_from_add.Add(person_res);
            }
            //print
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_add in person_resp_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_add.ToString());
            }



            //act
            List<PersonResponse> persons_list_from_search =await  _personGetterService.GetFilteredPersons(nameof(Person.Gender), "ma");
            //print persons lists from get
            _testOutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_get.ToString());
            }
            //assert
            persons_list_from_search.Should().OnlyContain(temp=>temp.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase));

            //foreach (var item in person_resp_list_from_add)
            //{
            //    if (item.PersonName != null)
            //    {
            //        if (item.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        {

            //            Assert.Contains(item, persons_list_from_search);
            //        }
            //    }
            //}
        }
        #endregion

        #region GetSortedPersons

        //when we sort based of person name in desc it should return persons list in decending order 
        [Fact]
        public async Task GetSortedPersons()
        {
            CountryAddRequest? countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? countryAddRequest2 = _fixture.Create<CountryAddRequest>();
         
            //CountryAddRequest? countryAddRequest1 = new CountryAddRequest()
            //{
            //    CountryName = "cleveland"
            //};
            //CountryAddRequest? countryAddRequest2 = new CountryAddRequest()
            //{
            //    CountryName = "arcitca"
            //};
            CountryResponse resp1 = await  _countryService.AddCountry(countryAddRequest1);
            CountryResponse resp2 = await _countryService.AddCountry(countryAddRequest2);
            PersonAddRequest? personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "kangana@gmail.com").With(t => t.PersonName, "marayman").With(t => t.CountryId, resp1.CountryId).Create();
            PersonAddRequest? personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "king@gmail.com").With(t => t.PersonName, "rahaman1").With(t => t.CountryId, resp1.CountryId).Create();
            PersonAddRequest? personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "Ashi").With(t => t.CountryId, resp2.CountryId).Create();

            //PersonAddRequest? personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "ashokboya",
            //    Email = "ashok12@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Male,
            //    Address = "KNL",
            //    ReceiveLetters = true,
            //    CountryId = resp1.CountryId,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),

            //};
            //PersonAddRequest? personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "kingmou",
            //    Email = "kings@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Other,
            //    Address = "KDP",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2002-01-01"),

            //};
            //PersonAddRequest? personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "mounika",
            //    Email = "mounika@gmail.com",
            //    Gender = ServiceContracts.Enums.GenderOptions.Female,
            //    Address = "KNlM",
            //    ReceiveLetters = true,
            //    CountryId = resp2.CountryId,
            //    DateOfBirth = DateTime.Parse("2003-01-01"),

            //};

            List<PersonAddRequest> request_lst = new
            List<PersonAddRequest>() { personAddRequest1,
                    personAddRequest2,personAddRequest3
            };

            List<PersonResponse> person_resp_list_from_add = new List<PersonResponse>();
            foreach (var item in request_lst)
            {

                PersonResponse person_res = await _personAdderService.AddPerson(item);
                person_resp_list_from_add.Add(person_res);
            }

            //print
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_add in person_resp_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_add.ToString());
            }


            List<PersonResponse> allpersons = await _personGetterService.GetAllPersons();
            //act
            List<PersonResponse> persons_list_from_sort= await _personSorterService.GetSortedPersons(allpersons,  nameof(Person.PersonName), SortOrderOptions.DESC);
            //print persons lists from get
            //_testOutputHelper.WriteLine("actual: ");
            //foreach (PersonResponse person_response_get in persons_list_from_sort)
            //{
            //    _testOutputHelper.WriteLine(person_response_get.ToString());
            //}
           // below  lines of code cmmntd cause sorting and name fingd can find in asserting line  below
           // person_resp_list_from_add = person_resp_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();




            //print persons lists from get
            _testOutputHelper.WriteLine("after sort: ");
            foreach (PersonResponse person_response_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_get.ToString());
            }
            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);

            //assert
           // below two lines of code cmmntd cause sorting and name fingd can find in asserting line  above
          //  persons_list_from_sort.Should().BeEquivalentTo(person_resp_list_from_add);

            //for (int i=0;i<person_resp_list_from_add.Count;i++)
            //{
            //    Assert.Equal(person_resp_list_from_add[i], persons_list_from_sort[i]);
            //}
        }
        #endregion

        #region updatePerson
        /// <summary>
        /// when we supply as personupdate it should throw argumentnullexception
        /// </summary>
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //arrange
        PersonUpdateRequest? personUpdateRequest = null;
            //act


            Func<Task> action = async () =>
            {
                await _personUpdaterServices.UpdatePerson(personUpdateRequest);

            };
            await action.Should().ThrowAsync<ArgumentNullException>();

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            // await    _personService.UpdatePerson(personUpdateRequest);
            //});

        }
        /// <summary>
        /// when we supply invalid personid it should thro argumentexception
        /// </summary>
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create() ;
            //new PersonUpdateRequest() { PersonID=Guid.NewGuid()};
            //act
            Func<Task> action = async () =>
            {
                await _personUpdaterServices.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();

            //await   Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //   await _personService.UpdatePerson(personUpdateRequest);
            //});

        }
        /// <summary>
        /// when person name is null it should throw argument exception
        /// </summary>
        [Fact]
        public async Task UpdatePerson_PersonNameIsNUll()
        {
            //arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName="barak"
            //};
            CountryResponse countryResponsefromAdd= await _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "Ashi").With(t => t.CountryId).Create();

            //    new PersonAddRequest()
            //{
            //    PersonName="ramu",CountryId=countryResponsefromAdd.CountryId,Email="ramu@gmail.com",Gender=GenderOptions.Female
            //};
            PersonResponse person_Response_fromAdd = await _personAdderService.AddPerson(personAddRequest);
            PersonUpdateRequest personUpdateRequest = person_Response_fromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //act

            Func<Task> action = async () =>    //or var action = async()=> also same
            {
                await _personUpdaterServices.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{

            //    await _personService.UpdatePerson(personUpdateRequest);
            //});



        }
        //first weill add new person and try to update the perosn name and email
        [Fact]
        public async  Task   UpdatePerson_PersonFullDetails()
        {
            //arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            //new CountryAddRequest()
            //{
            //    CountryName = "barak"
            //};
            CountryResponse countryResponsefromAdd = await _countryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "Ashi").With(t => t.CountryId).Create();

            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "ramu",
            //    CountryId = countryResponsefromAdd.CountryId,
            //    Address = "atp",
            //    Email="ramu@gmail.com",
            //    DateOfBirth = DateTime.Parse("2005-02-03"),
            //    Gender = GenderOptions.Male,
            //    ReceiveLetters = true

            //};
            PersonResponse person_Response_fromAdd = await  _personAdderService.AddPerson(personAddRequest);
            PersonUpdateRequest personUpdateRequest = person_Response_fromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "wiiliam";
            personUpdateRequest.Email = "William@gmail.com";
            personUpdateRequest.Address = "Antp";




            //act
              PersonResponse personResponse_from_Update= await _personUpdaterServices.UpdatePerson(personUpdateRequest);
              PersonResponse personResponse_from_get= await _personGetterService.GetPersonByPersonId(personResponse_from_Update.PersonId);

            personResponse_from_Update.Should().Be(personResponse_from_get);
            //Assert.Equal(personResponse_from_get,personResponse_from_Update);


        }
        #endregion

        #region DeletePerson
        //if you supply valid personid,it should return true
        [Fact]
        public async Task DeletePerson_validPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "USA"
            //};
            CountryResponse countryResponse_from_add= await _countryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "Ashi").With(t => t.CountryId).Create();

            //    new PersonAddRequest()
            //{
            //    PersonName="siva",Address="chittor",Email="siva@gmail.com",Gender=GenderOptions.Male,ReceiveLetters=true,
            //    CountryId=countryResponse_from_add.CountryId,DateOfBirth=DateTime.Parse("2007-03-04")
            //};

            PersonResponse personresponse_from_add = await _personAdderService.AddPerson(personAddRequest);

            //act
           bool isDeleted= await _personDeleterService.DeletePerson(personresponse_from_add.PersonId);

           // Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }

        //if you supply invalid personid,it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            //    new CountryAddRequest()
            //{
            //    CountryName = "USA"
            //};
            CountryResponse countryResponse_from_add = await _countryService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "as@gmail.com").With(t => t.PersonName, "Ashi").With(t => t.CountryId).Create();
            //    new PersonAddRequest()
            //{
            //    PersonName = "siva",
            //    Address = "chittor",
            //    Email = "siva@gmail.com",
            //    Gender = GenderOptions.Male,
            //    ReceiveLetters = true,
            //    CountryId = countryResponse_from_add.CountryId,
            //    DateOfBirth = DateTime.Parse("2007-03-04")
            //};

            PersonResponse personresponse_from_add = await _personAdderService.AddPerson(personAddRequest);

            //act
            bool isDeleted =await _personDeleterService.DeletePerson(Guid.NewGuid());

            isDeleted.Should().BeFalse();
           // Assert.False(isDeleted);
        }
        #endregion

    }
}
