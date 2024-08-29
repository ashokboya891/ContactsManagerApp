using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface ICountryService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? request);
       Task< List<CountryResponse>> GetAllCountryList();

       Task<CountryResponse?> GetCountryById(Guid? countryId);
        /// <summary>
        /// uploads countries from excel file into db
        /// </summary>
        /// <param name="formfile">Excel file with list of countries </param>
        /// <returns>return number of countries added</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formfile);
    }
}
