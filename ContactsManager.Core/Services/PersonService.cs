//using Entities;
//using ServiceContracts;
//using ServiceContracts.DTO;
//using System.ComponentModel.DataAnnotations;
//using Services.Helpers;
//using System.Linq.Expressions;
//using ServiceContracts.Enums;
//using System.Diagnostics;
//using Microsoft.EntityFrameworkCore;
//using CsvHelper;
//using System.Globalization;
//using CsvHelper.Configuration;
//using OfficeOpenXml;
//using RepositoryContracts;
//using Microsoft.Extensions.Logging;
//using Serilog;
//using SerilogTimings;
//using Exceptions;
//namespace Services
//{
//    public class PersonService : IPersonGetterService
//    {
//        private readonly IPersonRespository _personsRepository;
//        private readonly ILogger<PersonService> _logger;
//        private readonly IDiagnosticContext _diagnosticContext;
//        //we are able to get country name through person navigation property
//        //private readonly ICountryService _countryService;

//        //this line cmntd after using relations cause we are getting country info through foreignkey code
//        //private PersonResponse ConvertPersonToPersonResponse(Person person)
//        //{
//        //    PersonResponse person_response = person.ToPersonResponse();
//        //    //person_response.Country = _countryService.GetCountryById(person.CountryId)?.CountryName;
//        //    person_response.Country = person.Country?.CountryName;

//        //    return person_response;
//        //}

//        //public PersonService(ApplicationDbContext personsDbContext,ICountryService countryService)
//        public PersonService(IPersonRespository repo,ILogger<PersonService> logger,IDiagnosticContext diagnosticContext)
//        {
//            this._diagnosticContext= diagnosticContext;
//            _logger= logger;
//            _personsRepository =repo;
//            //_countryService = countryService;
////            if (initialize)
////            {
////                _db.Add(
////                 new Person()
////                 {
////                     PersonId = Guid.Parse("C0CA98AC-9EB4-42E3-B867-D9057EACF447"),
////                     PersonName = "Rubie",
////                     Email = "rarrighetti0@tamu.edu",
////                     DateOfBirth = DateTime.Parse("1991-06-30"),
////                     Gender = "Female",
////                     Address = "  0 Caliangt Center",
////                     ReceiveLetters = false,
////                     CountryId = Guid.Parse("F770F999-D5F1-4F77-AFB0-0C43C949452F"),
////                 });
////                _db.Add(
////                  new Person()
////                  {
////                      PersonId = Guid.Parse("120E1AB2-FC41-4BAF-A278-5885EA2A95D2"),
////                      PersonName = "Lucien",
////                      Email = "lhanhart1@altervista.org",
////                      DateOfBirth = DateTime.Parse("1999-12-21"),
////                      Gender = "Male",
////                      Address = " ,7 Westerfield Junction",
////                      ReceiveLetters = true,
////                      CountryId = Guid.Parse("90982989-5A80-4CB6-9AC2-BCCDB6880E72"),
////                  });

////                _db.Add(
////             new Person()
////             {
////                 PersonId = Guid.Parse("B57A0FFE-3BEA-4EEE-AA39-5B391A4C2CCD"),
////                 PersonName = "Sid",
////                 Email = "sgrushin2@google.com.au",
////                 DateOfBirth = DateTime.Parse("1991-08-10"),
////                 Gender = "Male",
////                 Address = " 66 Charing Cross Parkway",
////                 ReceiveLetters = true,
////                 CountryId = Guid.Parse("9D1F65F4-FC7A-43F1-B828-06D519ADF6BA"),

////             });
////                _db.Add(
////          new Person()
////          {
////              PersonId = Guid.Parse("DB520941-BF84-4043-8BE5-5BDB89250063"),
////              PersonName = "Dorita",
////              Email = "dbeaves3@myspace.com",
////              DateOfBirth = DateTime.Parse("1991-09-18"),
////              Gender = "Female",
////              Address = "8034 Reindahl Crossing",
////              ReceiveLetters = false,
////              CountryId = Guid.Parse("1CB1FC4A-E0C8-4F56-BF1A-A1E3D53DEE5B"),
////          });
////                _db.Add(
////       new Person()
////       {
////           PersonId = Guid.Parse("9F023E02-B8EF-46D3-8C32-234A53E34F92"),
////           PersonName = "Emilia",
////           Email = "equaltrough9@yelp.com",
////           DateOfBirth = DateTime.Parse("1995-04-08"),
////           Gender = "Female",
////           Address = "87 Spenser Terrace",
////           ReceiveLetters = true,
////           CountryId = Guid.Parse("F770F999-D5F1-4F77-AFB0-0C43C949452F"),

////       });
////                _db.Add(

////                new Person()
////                {
////                    PersonId = Guid.Parse("9D2E68AF-B5AC-49C6-93DA-01D781E0ED3D"),
////                    PersonName = "Pooh",
////                    Email = "pbagehot6@columbia.edu",
////                    DateOfBirth = DateTime.Parse("1992-11-27"),
////                    Gender = "Male",
////                    Address = "331 Delladonna Court",
////                    ReceiveLetters = true,
////                    CountryId = Guid.Parse("3729B7F2-125D-4141-84D3-EC3A01E061C6"),

////                });

////                 //C0CA98AC-9EB4-42E3-B867-D9057EACF447
////                //120E1AB2-FC41-4BAF-A278-5885EA2A95D2
////                //DB520941-BF84-4043-8BE5-5BDB89250063
////                //B57A0FFE-3BEA-4EEE-AA39-5B391A4C2CCD
////                //9F023E02-B8EF-46D3-8C32-234A53E34F92
////                //9D2E68AF-B5AC-49C6-93DA-01D781E0ED3D
////                /* 
////                 Rubie,rarrighetti0@tamu.edu,1991-06-30,Female,0 Caliangt Center,false
////Lucien,lhanhart1@altervista.org,1999-12-21,Male,7 Westerfield Junction,true
////Sid,sgrushin2@google.com.au,1991-08-10,Male,66 Charing Cross Parkway,true
////Dorita,dbeaves3@myspace.com,1991-09-18,Female,8034 Reindahl Crossing,false
////Darnall,ddockrey4@wordpress.org,1993-09-03,Male,34 Cottonwood Alley,false
////Herculie,hkondratenya5@icio.us,1996-11-01,Male,0220 Mesta Alley,true
////Pooh,pbagehot6@columbia.edu,1992-11-27,Male,331 Delladonna Court,true
////Fraze,fburril7@a8.net,2000-04-23,Male,13 Warbler Terrace,false
////Rosabel,rsleep8@scribd.com,1994-05-28,Female,4 Colorado Avenue,true
////Emilia,equaltrough9@yelp.com,1995-04-08,Female,87 Spenser Terrace,true
////*/
////            }
//        }



//        public  async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
//        {
//            if (personAddRequest == null)
//            {
//                throw new ArgumentNullException(nameof(personAddRequest));
//            }
//            //model validations
//            ValidationHelper.ModelValidation(personAddRequest);
//            ////validate personname
//            //if(string.IsNullOrEmpty(personAddRequest.PersonName))
//            //{
//            //    throw new ArgumentException("person name canot be blank");
//            //}


//            //covert personaddrequest into person type
//            Person person = personAddRequest.ToPerson();

//            //generzte person id
//            person.PersonId = Guid.NewGuid();

//            //add person object into persons lits
//            await _personsRepository.AddPerson(person);

//            // await _personsRepository.Persons.AddAsync(person);
//            //await _personsRepository.SaveChangesAsync();
//            //below line is for usign sp
//            //int n= _db.sp_InsertPerson(person);
//            //convert peroson object into personresponse type 
//            return person.ToPersonResponse();

//            //return person_response;

//        }
//        public async Task<List<PersonResponse>> GetAllPersons()
//        {

//            _logger.LogInformation("GEtALlpersons of personservice");

//            //we implicity including country cause we want relation between person and coutry table as foreign and primary key and in model we are using [foreignkey] 

//            var persons = await _personsRepository.GetAllPersons();
            
//            //select I* from persons;
//            //bewlo line cmntd after using sp from context class below lines are sp lines 
//            //return _db.Persons.ToList().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
//            return  persons.Select(temp => temp.ToPersonResponse()).ToList();


//            //return   _db.sp_GetAllPersons().
//            //   Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
//        }

//        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
//        {
//            if (personId == null)
//            {
//                return null;
//            }
//            //code added after relation added [foreignkey]
//            //EG:- person.Country.CountryName
//            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);
//                //.Persons.Include("Country").
//                //FirstOrDefaultAsync(tenp => tenp.PersonId == personId);
//            if (person == null) { return null; }
//            return person.ToPersonResponse();

//        }

//        public async Task<List<PersonResponse>> GetFilteredPersons(string serachBy, string? searchstring)
//        {

//            _logger.LogInformation("GetFilteredPersons of PersonsService");

//            List<Person> persons;

//            using (Operation.Time("Time for Filtered Persons from Database"))
//            {
//                persons = serachBy switch
//                {
//                    nameof(PersonResponse.PersonName) =>
//                     await _personsRepository.GetFilteredPersons(temp =>
//                     temp.PersonName.Contains(searchstring)),

//                    nameof(PersonResponse.Email) =>
//                     await _personsRepository.GetFilteredPersons(temp =>
//                     temp.Email.Contains(searchstring)),

//                    nameof(PersonResponse.DateOfBirth) =>
//                     await _personsRepository.GetFilteredPersons(temp =>
//                     temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchstring)),


//                    nameof(PersonResponse.Gender) =>
//                     await _personsRepository.GetFilteredPersons(temp =>
//                     temp.Gender.Contains(searchstring)),

//                    nameof(PersonResponse.CountryId) =>
//                     await _personsRepository.GetFilteredPersons(temp =>
//                     temp.Country.CountryName.Contains(searchstring)),

//                    nameof(PersonResponse.Address) =>
//                    await _personsRepository.GetFilteredPersons(temp =>
//                    temp.Address.Contains(searchstring)),

//                    _ => await _personsRepository.GetAllPersons()
//                };
//            } //end of "using block" of serilog timings

//            _diagnosticContext.Set("Persons", persons);

//            return persons.Select(temp => temp.ToPersonResponse()).ToList();
//            //_logger.LogInformation("getfilterred persons of personservice");
//            //List<PersonResponse> allpersons = await GetAllPersons();

//            //List<PersonResponse> matchingpersons = allpersons;

//            //if (string.IsNullOrEmpty(serachBy) && string.IsNullOrEmpty(searchstring))
//            //{
//            //    return matchingpersons;
//            //}
//            //using (Operation.Time("filtere started in personservices"))
//            //{
//            //    switch (serachBy)
//            //    {
//            //        case nameof(PersonResponse.PersonName):
//            //            matchingpersons = allpersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//            //            break;
//            //        case nameof(PersonResponse.Email):
//            //            matchingpersons = allpersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//            //            break;
//            //        case nameof(PersonResponse.DateOfBirth):
//            //            matchingpersons = allpersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true).ToList();
//            //            break;
//            //        case nameof(PersonResponse.Gender):
//            //            matchingpersons = allpersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//            //            break;

//            //        case nameof(PersonResponse.CountryId):
//            //            matchingpersons = allpersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//            //            break;
//            //        case nameof(PersonResponse.Address):
//            //            matchingpersons = allpersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchstring, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//            //            break;
//            //        default: matchingpersons = allpersons; break;
//            //    }
//            //}
//            //_diagnosticContext.Set("Persons", matchingpersons);
//            //return matchingpersons;
//            //List<PersonResponse> persons = searchBy switch
//            //{
//            //    nameof(PersonResponse.PersonName) =>
//            //      await _personsRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchstring, StringComparison.OrdinalIgnoreCase)),
//            //    nameof(PersonResponse.Email) =>
//            //      await _personsRepository.GetFilteredPersons(temp => temp.Email.Contains(searchstring, StringComparison.OrdinalIgnoreCase)),
//            //    nameof(PersonResponse.DateOfBirth) =>
//            //     await _personsRepository.GetFilteredPersons(temp => temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchstring, StringComparison.OrdinalIgnoreCase)),
//            //    nameof(PersonResponse.Gender) =>
//            //      await _personsRepository.GetFilteredPersons(temp => temp.Gender.Contains(searchstring, StringComparison.OrdinalIgnoreCase)),
//            //    nameof(PersonResponse.CountryId) =>
//            //      await _personsRepository.GetFilteredPersons(temp => temp.Country.CountryName.Contains(searchstring, StringComparison.OrdinalIgnoreCase)),
//            //    nameof(PersonResponse.Address) =>
//            //     await _personsRepository.GetFilteredPersons(temp => temp.Address.Contains(searchstring, StringComparison.OrdinalIgnoreCase)),

//            //    _ => await _personsRepository.GetAllPersons()
//        }

//        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortorder)
//        {

//            _logger.LogInformation("getsortedpersons in persons service");
//            if (string.IsNullOrEmpty(sortBy))
//            {
//                return allpersons;
//            }
//            List<PersonResponse> sortedpersons = (sortBy, sortorder)
//            switch
//            {
//                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
//                allpersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

//                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
//                 allpersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

//                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
//                  allpersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

//                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
//                allpersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

//                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
//                 allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),

//                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
//                allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),


//                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
//                allpersons.OrderBy(temp => temp.Age).ToList(),

//                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
//                allpersons.OrderByDescending(temp => temp.Age).ToList(),


//                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
//                allpersons.OrderBy(temp => temp.Gender).ToList(),

//                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
//                allpersons.OrderByDescending(temp => temp.Gender).ToList(),

//                (nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
//              allpersons.OrderBy(temp => temp.Country).ToList(),

//                (nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
//             allpersons.OrderByDescending(temp => temp.Country).ToList(),

//                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
//                allpersons.OrderBy(temp => temp.Address).ToList(),

//                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
//              allpersons.OrderByDescending(temp => temp.Address).ToList(),

//                (nameof(PersonResponse.ReceiveLetters), SortOrderOptions.ASC) =>
//               allpersons.OrderBy(temp => temp.ReceiveLetters).ToList(),

//                (nameof(PersonResponse.ReceiveLetters), SortOrderOptions.DESC) =>
//              allpersons.OrderByDescending(temp => temp.ReceiveLetters).ToList(),

//                _ => allpersons

//            };
//            return sortedpersons;
//        }

//        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
//        {
//            if (personUpdateRequest == null)
//            {
//                throw new ArgumentNullException(nameof(Person));
//            };

//            //validation
//            ValidationHelper.ModelValidation(personUpdateRequest);

//            //get matching person object to update
//            //Person? matchedperson =await _personsRepository.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonID);
//            Person? matchedperson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonID);

//            if (matchedperson == null)
//            {
//                //throw new ArgumentException("given person id doesnt not exists");
//                throw new InvalidPersonIDException("given person id doesn't not exists");

//            }
//            //update all details
//            matchedperson.PersonName = personUpdateRequest.PersonName;
//            matchedperson.Email = personUpdateRequest.Email;
//            matchedperson.Gender = personUpdateRequest.Gender.ToString();
//            matchedperson.CountryId = personUpdateRequest.CountryId;
//            matchedperson.Address = personUpdateRequest.Address;
//            matchedperson.ReceiveLetters = personUpdateRequest.ReceiveLetters;
//            matchedperson.DateOfBirth = personUpdateRequest.DateOfBirth;
//            await _personsRepository.UpdatePerson(matchedperson);
//            //update

//            //await   _personsRepository.SaveChangesAsync(); //update
//            return matchedperson.ToPersonResponse();




//        }

//        public async Task<bool> DeletePerson(Guid? personID)
//        {
//            if (personID == null)
//            {
//                throw new ArgumentNullException(nameof(personID));
//            }
//            // Person? person =await _personsRepository.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personID);
//            Person? person = await _personsRepository.GetPersonByPersonId(personID.Value);

//            if (person == null)
//                return false;

//            _personsRepository.DeletePersonByPersonID(personID.Value);
//            //_personsRepository.Persons.Remove(_personsRepository.Persons.First(temp => temp.PersonId == personID));
//            //await _personsRepository.SaveChangesAsync();
//            return true;
//        }

//        public async  Task<MemoryStream> GetPersonsCSV()
//        {
//            MemoryStream memorystream = new MemoryStream();
//            StreamWriter streamwriter = new StreamWriter(memorystream);  //stream writer writes contetn into memorystream
//            CsvConfiguration csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture); //.US,UK  also we can use as cultures
//                                                                                             //
//            CsvWriter csvwriter = new CsvWriter(streamwriter,csvconfig,leaveOpen:true);
//            //name,email,address,age pattern of lines
//            csvwriter.WriteField(nameof(PersonResponse.PersonName));
//            csvwriter.WriteField(nameof(PersonResponse.Email));
//            csvwriter.WriteField(nameof(PersonResponse.Address));
//            csvwriter.WriteField(nameof(PersonResponse.Gender));
//            csvwriter.WriteField(nameof(PersonResponse.ReceiveLetters));
//            csvwriter.WriteField(nameof(PersonResponse.Age));
//            csvwriter.WriteField(nameof(PersonResponse.Country));
//            csvwriter.WriteField(nameof(PersonResponse.DateOfBirth));





//            csvwriter.NextRecord(); //nextline 

//            List<PersonResponse> resp = await GetAllPersons();
                
//            //List<PersonResponse> resp=   await _personsRepository.Persons.Include("Country").
//            //   Select(temp=>temp.ToPersonResponse()).ToListAsync();  
//            //await csvwriter.WriteRecordsAsync(resp); //1,"ashok,""city","24","true"'  data rows

//            foreach (PersonResponse item in resp)
//            {
//                csvwriter.WriteField(item.PersonName);
//                csvwriter.WriteField(item.Email);
//                csvwriter.WriteField(item.Address);
//                csvwriter.WriteField(item.Gender);
//                csvwriter.WriteField(item.ReceiveLetters);
//                csvwriter.WriteField(item.Age);
//                csvwriter.WriteField(item.Country);
//                if(item.DateOfBirth.HasValue)
//                {

//                csvwriter.WriteField(item.DateOfBirth.Value.ToString("yyyy-mm-dd"));
//                }
//                else
//                {
//                    csvwriter.WriteField("");

//                }
//                csvwriter.NextRecord();
//                csvwriter.Flush();
//            }
//            memorystream.Position = 0;  //after readig all data it will wait at end point  so we are returning after reaching to zero position
//            return memorystream;
//        }

//        public async Task<MemoryStream> GetPersonsExc()
//        {
//            MemoryStream memorystream = new MemoryStream();
//            using(ExcelPackage excelpackage=new ExcelPackage(memorystream))
//            {
//                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets.Add("PersonSheet");
//                worksheet.Cells["A1"].Value = "Person Name";
//                worksheet.Cells["B1"].Value = "Email";
//                worksheet.Cells["C1"].Value = "Age";
//                worksheet.Cells["D1"].Value = "Address";
//                worksheet.Cells["E1"].Value = "Gender";
//                worksheet.Cells["F1"].Value = "Country";
//                worksheet.Cells["G1"].Value = "Date Of Birth";
//                worksheet.Cells["H1"].Value = "Received Letters"; 
//                using(ExcelRange headercells = worksheet.Cells["A1:H1"]) //using block will invoke dispose colletor after end of exceution of using block
//                {
//                    headercells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
//                    headercells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
//                    headercells.Style.Font.Bold = true;
//                }
//                int row = 2;
//                List<PersonResponse> resp = await GetAllPersons();

//                //List<PersonResponse> resp = _personsRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();
//                foreach (PersonResponse personresponse in resp)
//                {
//                    worksheet.Cells[row, 1].Value= personresponse.PersonName;
//                    worksheet.Cells[row, 2].Value = personresponse.Email;
//                    worksheet.Cells[row, 3].Value = personresponse.Age;
//                    worksheet.Cells[row, 4].Value = personresponse.Address;
//                    worksheet.Cells[row, 5].Value = personresponse.Gender;
//                    worksheet.Cells[row, 6].Value = personresponse.Country;
//                    if(personresponse.DateOfBirth.HasValue)
//                          worksheet.Cells[row, 7].Value = personresponse.DateOfBirth.Value.ToString("yyyy-mm-dd");
//                    worksheet.Cells[row, 8].Value = personresponse.ReceiveLetters;
//                    row++;
//                }
//                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
//                await excelpackage.SaveAsync();
//            }
//            memorystream.Position = 0;
//            return memorystream;
//        }
//    }
//}
