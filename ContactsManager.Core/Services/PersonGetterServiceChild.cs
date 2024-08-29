using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonGetterServiceChild:PersonGetterService
    {
        public PersonGetterServiceChild(IPersonRespository repo, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext) : base(repo, logger, diagnosticContext)
        {
        }

        public override async Task<MemoryStream> GetPersonsExc()
        {
            MemoryStream memorystream = new MemoryStream();
            using (ExcelPackage excelpackage = new ExcelPackage(memorystream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets.Add("PersonSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Age";
               
                using (ExcelRange headercells = worksheet.Cells["A1:C1"]) //using block will invoke dispose colletor after end of exceution of using block
                {
                    headercells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headercells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headercells.Style.Font.Bold = true;
                }
                int row = 2;
                List<PersonResponse> resp = await GetAllPersons();

                if (resp.Count == 0)
                {
                    throw new InvalidOperationException("NO person data");  //vialoating LSP here cause this exception not there in parent class96
                }


                foreach (PersonResponse personresponse in resp)
                {
                    worksheet.Cells[row, 1].Value = personresponse.PersonName;
                    worksheet.Cells[row, 2].Value = personresponse.Email;
                    worksheet.Cells[row, 3].Value = personresponse.Age;
                
                    row++;
                }
                worksheet.Cells[$"A1:C{row}"].AutoFitColumns();
                await excelpackage.SaveAsync();
            }
            memorystream.Position = 0;
            return memorystream;
        }
    }
}
