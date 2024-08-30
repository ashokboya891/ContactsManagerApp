using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using FluentAssertions;
namespace TestUnits
{
    public class CountryTestsService
    {
        private readonly ICountryService _countryService;
        public CountryTestsService()
        {
            var countriesIntialDate=new List<Country>() { };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );
           ApplicationDbContext dbContext= dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesIntialDate);
            this._countryService = new CountryService(null);
        }
        #region Addcountry
        [Fact]
        public async Task AddCountry_NullCountry()
        {

            CountryAddRequest? request = null;
            Func<Task> action = async () =>
            {
                await _countryService.AddCountry(request);

            };
            await action.Should().ThrowAsync<ArgumentNullException>();

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            // {
            //    await _countryService.AddCountry(request);
            // });
        }
        [Fact]
        public async Task AddCountry_CountryIsNull()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            Func<Task> action = async () =>
            {
                await _countryService.AddCountry(request);

            };
            await action.Should().ThrowAsync<ArgumentNullException>();

            //await  Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //  await  _countryService.AddCountry(request);
            //});
        }
        [Fact]
        public async Task AddCountry_DuplicateCountryName() 
        {
            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "end"
            };
            CountryAddRequest request2 = new CountryAddRequest()
            {
                CountryName = "enkd"
            };
           await _countryService.AddCountry(request2 );
           await _countryService.AddCountry(request1 );


        }
        [Fact]
        public async Task Addcountry_ProperCountryResults()
        {
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "fadv"
            };
            CountryResponse resp=await _countryService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries =await _countryService.GetAllCountryList();
            //resp.Should().con(temp => temp.PersonName); 
            //resp.Should().Be(Guid.NewGuid);
             Assert.True(resp.CountryId != Guid.Empty);
           // countries_from_GetAllCountries.Should().NotContain(countries_from_GetAllCountries);
            Assert.Contains(resp,countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_countrys_resp_list = await _countryService.GetAllCountryList();
            //Assert.Empty(actual_countrys_resp_list);
            actual_countrys_resp_list.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> lst = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){CountryName="ind"},
                new CountryAddRequest(){CountryName="russia"}
            };
            List<CountryResponse> country_list_from_add_country = new List<CountryResponse>();

            foreach (var item in lst)
            {
                country_list_from_add_country.Add(await _countryService.AddCountry(item));
            }
            List<CountryResponse> actual_country_resp_list=await _countryService.GetAllCountryList();

            actual_country_resp_list.Should().BeEquivalentTo(country_list_from_add_country);
            //foreach (CountryResponse expectedcountry in country_list_from_add_country)
            //{
            //    Assert.Contains(expectedcountry, actual_country_resp_list);
            //}
        }
        #endregion

        #region GetCountryById
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            Guid? countryid = null;
            CountryResponse? countryresp_from_get_method=await _countryService.GetCountryById(countryid);
            countryresp_from_get_method.Should().BeNull();  
           // Assert.Null(countryresp_from_get_method);
        }
        [Fact]
        public async Task GetCountrybyCountryId_ValidByCountryId()
        {
            CountryAddRequest countryadd = new CountryAddRequest()
            {
                CountryName ="china"
            };
            CountryResponse countryResponse_from_add = await  _countryService.AddCountry(countryadd);
            CountryResponse? countryresp_from_get=await _countryService.GetCountryById(countryResponse_from_add.CountryId);
            countryresp_from_get.Should().BeEquivalentTo(countryResponse_from_add);
            //Assert.Equal(countryResponse_from_add,countryresp_from_get);
        }
        #endregion
    }
}
