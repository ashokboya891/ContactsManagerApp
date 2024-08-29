using CsvHelper.Expressions;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountrysRepository _countriesRepository;

        //private readonly ApplicationDbContext _db;
        //private readonly ApplicationDbContext _context;


        public CountryService(ICountrysRepository repo)
        {
            _countriesRepository = repo;
            //if (initilize)
            //{
            //    _db.AddRange(new List<Country>()
            //    {
            //    new Country()
            //    {
            //        CountryId = Guid.Parse("F770F999-D5F1-4F77-AFB0-0C43C949452F"),
            //        CountryName = "BHARTH"
            //    },
            //    new Country()
            //    {
            //        CountryId = Guid.Parse("90982989-5A80-4CB6-9AC2-BCCDB6880E72"),
            //        CountryName = "SRILANKA"
            //    },
            //    new Country()
            //    {
            //        CountryId = Guid.Parse("9D1F65F4-FC7A-43F1-B828-06D519ADF6BA"),
            //        CountryName = "bangaldesh"
            //    },
            //    new Country()
            //    {
            //        CountryId = Guid.Parse("1CB1FC4A-E0C8-4F56-BF1A-A1E3D53DEE5B"),
            //        CountryName = "nepal"

            //    },
            //    new Country()
            //    {

            //        CountryId = Guid.Parse("3729B7F2-125D-4141-84D3-EC3A01E061C6"),
            //        CountryName = "bali"

            //    }
            //  });
            //    //F770F999-D5F1-4F77-AFB0-0C43C949452F
            //    //90982989-5A80-4CB6-9AC2-BCCDB6880E72
            //    //9D1F65F4-FC7A-43F1-B828-06D519ADF6BA
            //    //1CB1FC4A-E0C8-4F56-BF1A-A1E3D53DEE5B
            //    //3729B7F2-125D-4141-84D3-EC3A01E061C6
            //}
            //_context = context;
        }


        public async Task<CountryResponse> AddCountry(CountryAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (request.CountryName == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            //if (await _countriesRepository.Countries.CountAsync(temp => temp.CountryName == request.CountryName) > 0)
            if (await _countriesRepository.GetCountryByCountrName(request.CountryName)!=null)

            {
                throw new ArgumentException("country name already exists");
            }
            Country country = request.ToCountry();
            country.CountryId = Guid.NewGuid();

            await _countriesRepository.AddCountry(country);

            //await _countriesRepository.Countries.AddAsync(country);
           // await _countriesRepository.SaveChangesAsync();   //awit is used to hold exection without executing above linr it cant reach below line so order it follow 
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountryList()
        {
           // List<Country> countries = await _countriesRepository.GetAllCountries();
            return  (await
                  _countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();

            //countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            if (countryId == null) return null;

            Country? country_resp_from_list = await _countriesRepository.GetCountryByCOuntryId(countryId.Value);
            //  _countriesRepository.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryId);

            if (country_resp_from_list == null) return null;

            return country_resp_from_list.ToCountryResponse();


        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formfile)
        {
            MemoryStream memorystream = new MemoryStream();
            await formfile.CopyToAsync(memorystream);
            int countriesinserted = 0;
            using (ExcelPackage excelpackage = new ExcelPackage(memorystream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets["Countries"];

                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellvalue = Convert.ToString(worksheet.Cells[row, 1].Value);
                    if (cellvalue != null)
                    {
                        string? Name = cellvalue.ToString();
                        //if (_countriesRepository.Persons.Where(t => t.PersonName == Name).Count() == 0)
                        if (await _countriesRepository.GetCountryByCountrName(Name)==null)

                        {
                            Country n = new Country { CountryName = Name };
                             await   _countriesRepository.AddCountry(n);

                            //_countriesRepository.Countries.Add(n);
                            //await _countriesRepository.SaveChangesAsync();
                            countriesinserted++;

                        }
                    }
                }
            }
            return countriesinserted;


        }
    }
}
