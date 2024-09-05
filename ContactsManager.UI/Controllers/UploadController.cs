using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDE.Controllers
{
    [Route("Upload")]
    public class UploadController : Controller
    {
        private readonly ICountryService countryService;

        public UploadController(ICountryService countryService)
        {
            this.countryService = countryService;
        }
        [Route("UploadExcelForm")]
        public IActionResult UploadExcelForm()
        {
            return View();
        }

        [HttpPost]
        [Route("UploadExcelForm")]

        public async Task<IActionResult> UploadExcelForm(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length==0) 
            {
                ViewBag.ErrorMessage = "please select & upload excel file";
                return View();

            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx",StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "unsupported  file xlsx is expected";
                return View();
            }
            int count=await countryService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Message = $"{count} number of countries uploaded";
            return View();


        }
    }
}
