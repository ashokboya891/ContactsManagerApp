using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;
            
            CountryResponse countryresp_compare1 = (CountryResponse)obj; 
            return this.CountryId==countryresp_compare1.CountryId &&
                this.CountryName==countryresp_compare1.CountryName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse { CountryId = country.CountryId, CountryName = country.CountryName };
        }
    }
}
