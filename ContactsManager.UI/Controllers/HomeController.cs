using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDE.Controllers
{
    [AllowAnonymous]  //it states that without authentication or logged in we can use all method which are present in it

    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
           IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error!=null)
            {
                ViewBag.ErrorMessage=exceptionHandlerPathFeature.Error.Message;

            }
            return View();  //views/shared/error
        }
    }
}
