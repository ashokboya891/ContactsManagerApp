using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDE.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[Controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Register(RegisterDTO registerDTO)
        {
            //checking validation error
            if (ModelState.IsValid==false) 
            {
                ViewBag.Errors=ModelState.Values.SelectMany(t=>t.Errors).Select(t=>t.ErrorMessage);
                return View(registerDTO);
            }
            //used to store user register info into identity database
            ApplicationUser user = new ApplicationUser()
            {
                Email=registerDTO.Email,
                PhoneNumber=registerDTO.Phone,
                UserName=registerDTO.Email,
                PersonName=registerDTO.PersonName

            };
            IdentityResult result=await _userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                   
                }
            return View(registerDTO);
            }
        }
    }
}
