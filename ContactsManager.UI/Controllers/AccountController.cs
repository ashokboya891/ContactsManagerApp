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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                //sign in 
                await _signInManager.SignInAsync(user,isPersistent: false);   //creates cookie with encrypted user details for authenticator to read in program.cs file  stores application cookie in dev tool browser to keep login if is true if not it will be closed if we close chorme 
                
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
