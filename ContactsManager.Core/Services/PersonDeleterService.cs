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
    public class PersonDeleterService  : IPersonDeleterService
    {
        private readonly IPersonRespository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

    
        public PersonDeleterService(IPersonRespository repo,ILogger<PersonGetterService> logger,IDiagnosticContext diagnosticContext)
        {
            this._diagnosticContext= diagnosticContext;
            _logger= logger;
            _personsRepository =repo;

    
        }
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }
    
            Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);

            if (person == null)
                return false;

            _personsRepository.DeletePersonByPersonID(personID.Value);
    
            return true;
        }

    }
}
