using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using System.Linq.Expressions;
using ServiceContracts.Enums;
using System.Diagnostics;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;
namespace Services
{
    public class PersonSorterService : IPersonSorterService
    {
        private readonly IPersonRespository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonSorterService(IPersonRespository repo, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            this._diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = repo;

        }
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortorder)
        {

            _logger.LogInformation("getsortedpersons in persons service");
            if (string.IsNullOrEmpty(sortBy))
            {
                return allpersons;
            }
            List<PersonResponse> sortedpersons = (sortBy, sortorder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
                allpersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
                 allpersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
                  allpersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
                allpersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
                 allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
                allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),


                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
                allpersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
                allpersons.OrderByDescending(temp => temp.Age).ToList(),


                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
                allpersons.OrderBy(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
                allpersons.OrderByDescending(temp => temp.Gender).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
              allpersons.OrderBy(temp => temp.Country).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
             allpersons.OrderByDescending(temp => temp.Country).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
                allpersons.OrderBy(temp => temp.Address).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
              allpersons.OrderByDescending(temp => temp.Address).ToList(),

                (nameof(PersonResponse.ReceiveLetters), SortOrderOptions.ASC) =>
               allpersons.OrderBy(temp => temp.ReceiveLetters).ToList(),

                (nameof(PersonResponse.ReceiveLetters), SortOrderOptions.DESC) =>
              allpersons.OrderByDescending(temp => temp.ReceiveLetters).ToList(),

                _ => allpersons

            };
            return sortedpersons;
        }
    }
}    