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
    public class PersonAdderService : IPersonAdderService
    {
        private readonly IPersonRespository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPersonRespository repo,ILogger<PersonGetterService> logger,IDiagnosticContext diagnosticContext)
        {
            this._diagnosticContext= diagnosticContext;
            _logger= logger;
            _personsRepository =repo;
       
        }    
        public  async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }    
            ValidationHelper.ModelValidation(personAddRequest);    
            Person person = personAddRequest.ToPerson();    
            person.PersonId = Guid.NewGuid();    
            await _personsRepository.AddPerson(person);    
            return person.ToPersonResponse();    
        }
       
    }
}
