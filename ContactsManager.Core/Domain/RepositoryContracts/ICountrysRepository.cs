using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
        /// <summary>
        /// represents data access logic for managing person entity
        /// </summary>
        /// <returns></returns>
    public  interface ICountrysRepository
    {
        /// <summary>
        /// adds new country object to data store
        /// </summary>
        /// <param name="Country">country object to add</param>
        /// <returns>return the country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country Country);
        /// <summary>
        /// returns all the countries list from data store
        /// </summary>
        /// <returns>all countruies from table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// return the country object based on the given country id;otherwise it will return null
        /// </summary>
        /// <param name="id">country id to search</param>
        /// <returns>returns maching person or null</returns>
        Task<Country?> GetCountryByCOuntryId(Guid id);

        /// <summary>
        /// rerurn a country object based on the given country name
        /// </summary>
        /// <param name="countryName">country name to search</param>
        /// <returns>Matching person or null</returns>
        Task<Country?> GetCountryByCountrName(string countryName);

    }
}
