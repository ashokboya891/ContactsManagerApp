﻿using ContactsManager.Core.Domain.IdentityEntities;
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //it will remove identity cookie from developer tool in browser so as long as cookie preset in chrome it will consider as loged in account
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
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
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            ModelState.AddModelError("Login", "Invalid email or Password");
            return View(loginDTO);
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
