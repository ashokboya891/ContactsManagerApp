using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsGetterServiceWithFewExcelFields : IPersonGetterService
    {
        private readonly PersonGetterService _personGetterService;
        public PersonsGetterServiceWithFewExcelFields(PersonGetterService personGetterService)
        {
            _personGetterService = personGetterService;
        }
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string serachBy, string? searchstring)
        {
            return await _personGetterService.GetFilteredPersons(serachBy, searchstring);
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            return await _personGetterService.GetPersonByPersonId(personId);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personGetterService.GetPersonsCSV();
        }

        public  async Task<MemoryStream> GetPersonsExc()
        {

            MemoryStream memorystream = new MemoryStream();
            using (ExcelPackage excelpackage = new ExcelPackage(memorystream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets.Add("PersonSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Age";
                worksheet.Cells["D1"].Value = "Gender";
                worksheet.Cells["E1"].Value = "Country";
                using (ExcelRange headercells = worksheet.Cells["A1:E1"]) //using block will invoke dispose colletor after end of exceution of using block
                {
                    headercells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headercells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headercells.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> resp = await GetAllPersons();


                foreach (PersonResponse personresponse in resp)
                {
                    worksheet.Cells[row, 1].Value = personresponse.PersonName;
                    worksheet.Cells[row, 2].Value = personresponse.Email;
                    worksheet.Cells[row, 3].Value = personresponse.Age;
                    worksheet.Cells[row, 4].Value = personresponse.Gender;
                    worksheet.Cells[row, 5].Value = personresponse.Country;
                    row++;
                }
                worksheet.Cells[$"A1:E{row}"].AutoFitColumns();
                await excelpackage.SaveAsync();
            }
            memorystream.Position = 0;
            return memorystream;
        }
    }
}
