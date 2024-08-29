using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public  class CountriesRepository : ICountrysRepository
    {
        private readonly ApplicationDbContext _db;
        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async  Task<Country> AddCountry(Country Country)
        {
            _db.Countries.Add(Country);
            await _db.SaveChangesAsync();
            return Country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
            
        }

        public async Task<Country?> GetCountryByCountrName(string countryName)
        {
           return await _db.Countries.FirstOrDefaultAsync(t=>t.CountryName==countryName);
        }

        public async Task<Country?> GetCountryByCOuntryId(Guid countryid)
        {
          return await  _db.Countries.FirstOrDefaultAsync(t => t.CountryId == countryid);

        }
    }
}
