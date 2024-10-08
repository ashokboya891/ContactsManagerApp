﻿using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnits
{
    public class PersonsControllerIntergationTest:IClassFixture<CustomWebApplicationFactory>
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
            HttpResponseMessage response=await _client.GetAsync("/Persons/Index");
            response.Should().BeSuccessful();
            string responseBody= await response.Content.ReadAsStringAsync();
            HtmlDocument html = new HtmlDocument();

            html.LoadHtml(responseBody);
            var document = html.DocumentNode;
            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }
        #endregion
    }
}
