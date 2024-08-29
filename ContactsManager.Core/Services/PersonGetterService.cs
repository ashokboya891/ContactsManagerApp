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
    public class PersonGetterService : IPersonGetterService
    {
        private readonly IPersonRespository _personsRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;


        public PersonGetterService(IPersonRespository repo, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            this._diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = repo;

        }
        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {

            _logger.LogInformation("GEtALlpersons of personservice");
            var persons = await _personsRepository.GetAllPersons();
            return  persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public virtual async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }
    
    
            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);
    
    
            if (person == null) { return null; }
            return person.ToPersonResponse();

        }

        public virtual async Task<List<PersonResponse>> GetFilteredPersons(string serachBy, string? searchstring)
        {

            _logger.LogInformation("GetFilteredPersons of PersonsService");

            List<Person> persons;

            using (Operation.Time("Time for Filtered Persons from Database"))
            {
                persons = serachBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.PersonName.Contains(searchstring)),

                    nameof(PersonResponse.Email) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Email.Contains(searchstring)),

                    nameof(PersonResponse.DateOfBirth) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchstring)),


                    nameof(PersonResponse.Gender) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Gender.Contains(searchstring)),

                    nameof(PersonResponse.CountryId) =>
                     await _personsRepository.GetFilteredPersons(temp =>
                     temp.Country.CountryName.Contains(searchstring)),

                    nameof(PersonResponse.Address) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                    temp.Address.Contains(searchstring)),

                    _ => await _personsRepository.GetAllPersons()
                };
            } //end of "using block" of serilog timings

            _diagnosticContext.Set("Persons", persons);

            return persons.Select(temp => temp.ToPersonResponse()).ToList();
    
    
        }

        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memorystream = new MemoryStream();
            StreamWriter streamwriter = new StreamWriter(memorystream);  //stream writer writes contetn into memorystream
            CsvConfiguration csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture); //.US,UK  also we can use as cultures
    
            CsvWriter csvwriter = new CsvWriter(streamwriter,csvconfig,leaveOpen:true);
    
            csvwriter.WriteField(nameof(PersonResponse.PersonName));
            csvwriter.WriteField(nameof(PersonResponse.Email));
            csvwriter.WriteField(nameof(PersonResponse.Address));
            csvwriter.WriteField(nameof(PersonResponse.Gender));
            csvwriter.WriteField(nameof(PersonResponse.ReceiveLetters));
            csvwriter.WriteField(nameof(PersonResponse.Age));
            csvwriter.WriteField(nameof(PersonResponse.Country));
            csvwriter.WriteField(nameof(PersonResponse.DateOfBirth));





            csvwriter.NextRecord(); //nextline 

            List<PersonResponse> resp = await GetAllPersons();
                
    
    
    

            foreach (PersonResponse item in resp)
            {
                csvwriter.WriteField(item.PersonName);
                csvwriter.WriteField(item.Email);
                csvwriter.WriteField(item.Address);
                csvwriter.WriteField(item.Gender);
                csvwriter.WriteField(item.ReceiveLetters);
                csvwriter.WriteField(item.Age);
                csvwriter.WriteField(item.Country);
                if(item.DateOfBirth.HasValue)
                {

                csvwriter.WriteField(item.DateOfBirth.Value.ToString("yyyy-mm-dd"));
                }
                else
                {
                    csvwriter.WriteField("");

                }
                csvwriter.NextRecord();
                csvwriter.Flush();
            }
            memorystream.Position = 0;  //after readig all data it will wait at end point  so we are returning after reaching to zero position
            return memorystream;
        }

        public virtual async Task<MemoryStream> GetPersonsExc()
        {
            MemoryStream memorystream = new MemoryStream();
            using(ExcelPackage excelpackage=new ExcelPackage(memorystream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets.Add("PersonSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Age";
                worksheet.Cells["D1"].Value = "Address";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Date Of Birth";
                worksheet.Cells["H1"].Value = "Received Letters"; 
                using(ExcelRange headercells = worksheet.Cells["A1:H1"]) //using block will invoke dispose colletor after end of exceution of using block
                {
                    headercells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headercells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headercells.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> resp = await GetAllPersons();

    
                foreach (PersonResponse personresponse in resp)
                {
                    worksheet.Cells[row, 1].Value= personresponse.PersonName;
                    worksheet.Cells[row, 2].Value = personresponse.Email;
                    worksheet.Cells[row, 3].Value = personresponse.Age;
                    worksheet.Cells[row, 4].Value = personresponse.Address;
                    worksheet.Cells[row, 5].Value = personresponse.Gender;
                    worksheet.Cells[row, 6].Value = personresponse.Country;
                    if(personresponse.DateOfBirth.HasValue)
                          worksheet.Cells[row, 7].Value = personresponse.DateOfBirth.Value.ToString("yyyy-mm-dd");
                    worksheet.Cells[row, 8].Value = personresponse.ReceiveLetters;
                    row++;
                }
                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelpackage.SaveAsync();
            }
            memorystream.Position = 0;
            return memorystream;
        }
    }
}
