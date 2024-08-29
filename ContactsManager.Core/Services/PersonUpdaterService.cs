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
    public class PersonUpdaterService : IPersonUpdaterServices
    {
        private readonly IPersonRespository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
    
    
        public PersonUpdaterService(IPersonRespository repo,ILogger<PersonGetterService> logger,IDiagnosticContext diagnosticContext)
        {
            this._diagnosticContext= diagnosticContext;
            _logger= logger;
            _personsRepository =repo;
        
        }


        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            };

    
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchedperson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonID);

            if (matchedperson == null)
            {

                throw new InvalidPersonIDException("given person id doesn't not exists");
            }
            matchedperson.PersonName = personUpdateRequest.PersonName;
            matchedperson.Email = personUpdateRequest.Email;
            matchedperson.Gender = personUpdateRequest.Gender.ToString();
            matchedperson.CountryId = personUpdateRequest.CountryId;
            matchedperson.Address = personUpdateRequest.Address;
            matchedperson.ReceiveLetters = personUpdateRequest.ReceiveLetters;
            matchedperson.DateOfBirth = personUpdateRequest.DateOfBirth;
            await _personsRepository.UpdatePerson(matchedperson);

            return matchedperson.ToPersonResponse();

        }

      
    }
}
