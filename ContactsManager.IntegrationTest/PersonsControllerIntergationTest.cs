using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnits
{
    public class PersonsControllerIntergationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonsControllerIntergationTest(CustomWebApplicationFactory factory)
        {

            _client = factory.CreateClient();
        }
        #region Index
        [Fact]
        public async void Index_ToReturnView()
        {
            //arrange

            //act
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");
            response.Should().BeSuccessful();
            string responseBody = await response.Content.ReadAsStringAsync();
            HtmlDocument html = new HtmlDocument();

            html.LoadHtml(responseBody);
            var document = html.DocumentNode;
            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }
        #endregion
    }
}


//using Entities;
//using FluentAssertions;
//using HtmlAgilityPack;
//using TestUnits;

//public class PersonsControllerIntergationTest : IClassFixture<CustomWebApplicationFactory>
//{
//    private readonly HttpClient _client;
//    private readonly ApplicationDbContext _context;

//    public PersonsControllerIntergationTest(CustomWebApplicationFactory factory)
//    {
//        _client = factory.CreateClient();
//        _context = factory.Services.GetRequiredService<ApplicationDbContext>();  // Get the in-memory context
//    }

//    #region Index
//    [Fact]
//    public async void Index_ToReturnView()
//    {
//        // Arrange: Optionally, you can add some test data to the in-memory database.
//        _context.Persons.Add(new Person { Name = "John Doe", Age = 30 });
//        _context.SaveChanges();

//        // Act: Make the HTTP request
//        HttpResponseMessage response = await _client.GetAsync("/Persons/Index");
//        response.Should().BeSuccessful();

//        // Assert: Check the response and the data in the in-memory database
//        string responseBody = await response.Content.ReadAsStringAsync();
//        HtmlDocument html = new HtmlDocument();
//        html.LoadHtml(responseBody);
//        var document = html.DocumentNode;

//        // Verify that the table with class "persons" is in the HTML
//        document.QuerySelectorAll("table.persons").Should().NotBeNull();

//        // Verify that the person data is actually in the database
//        var person = _context.Persons.FirstOrDefault(p => p.Name == "John Doe");
//        person.Should().NotBeNull();
//        person.Age.Should().Be(30);
//    }
//    #endregion
//}
