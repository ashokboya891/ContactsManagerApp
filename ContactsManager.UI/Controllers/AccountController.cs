using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDE.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ContactsManager.UI.Controllers
{
    [Route("[Controller]/[action]")]
   // [AllowAnonymous]  //it states that without authentication or logged in we can use all method which are present in it
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //it will remove identity cookie from developer tool in browser so as long as cookie preset in chrome it will consider as loged in account
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        [HttpGet]
        [Authorize("NotAuthorized")]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]

        public async Task<IActionResult> Login(LoginDTO loginDTO,string? ReturnUrl)
        {
            //checking validation error
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).Select(t => t.ErrorMessage);
                return View(loginDTO);
            }
            var result=  await _signInManager.PasswordSignInAsync(loginDTO.Email,loginDTO.Password,isPersistent:false,lockoutOnFailure:false);//if provided credentials are matched in db it will creates identity token for that user   //if user entered 3 times failed credentials  for a while it wont allow to login that user from browser
               
            if(result.Succeeded)
            {
                //Admin
                ApplicationUser user=await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if(await _userManager.IsInRoleAsync(user,UserTypeOptions.Admin.ToString())) //SELECT CASE WHEN EXISTS( SELECT 1 FROM AspNetUserRoles ur JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = @UserId  AND r.Name = @RoleName
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            ModelState.AddModelError("Login", "Invalid email or Password");
            return View(loginDTO);
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
       // [ValidateAntiForgeryToken]

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
                //adding role to user specific 
                if(registerDTO.UserType==Core.Enums.UserTypeOptions.Admin)
                {
                   //create admin role
                   if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationrole = new ApplicationRole()
                        {
                            Name=UserTypeOptions.Admin.ToString()
                        };
                         await _roleManager.CreateAsync(applicationrole);
                    }
                    //add the new user into admin role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationrole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString()
                        };
                        await _roleManager.CreateAsync(applicationrole);
                    }
                    //add the new user into user role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());

                }
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
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
          ApplicationUser user=  await _userManager.FindByEmailAsync(email);
            if(user==null)
            {
                return Json(true);  //valid mail to register this email
            }
            else
            {
                return Json(false);  //already present email in db
            }

        }
    }
}
