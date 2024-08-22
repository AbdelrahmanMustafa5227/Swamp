using Microsoft.AspNetCore.Mvc;
using _Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Models;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;
using Microsoft.AspNetCore.Antiforgery;
using System;
using _DataAccess.Repository;
using _DataAccess.Repository.IRepository;

namespace Core.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment webHostEnvironment,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Registers()
        {
            RegisterationVM vm = new RegisterationVM();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Registers(RegisterationVM newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Fullname = newUser.Fullname;
                user.Email = newUser.Email;
                user.UserName = newUser.Email;
                user.Bio = newUser.Bio;
                user.BirthDate = newUser.Birthdate;
                user.ProfilePicUrl = "default/default1.png";
                user.ProfilePicMargin = 0;
                user.BackgroundPicUrl = "default.jpg";


                var result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ChangePic", "Profile" , new {area = "User"});
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty , item.Description);
                    }
                }
            }
            return View("Registers",newUser);
        }

        public IActionResult Logins()
        {
            LoginVM vm = new LoginVM();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logins(LoginVM loginVM , string? returnUrl)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email,loginVM.Password, loginVM.RememberMe , lockoutOnFailure:false);
                if(result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                
            }
            return View(loginVM);
        }

        public async Task<IActionResult> Logouts()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

        public IActionResult ForgetPass()
        {
            return View();
        }

        public IActionResult _ForgetPass(string email , DateOnly date)
        {
            DateOnly bDay = _unitOfWork.ApplicationRepo.Get(x=>x.UserName ==  email).BirthDate;

            if(bDay == date)
            {
                HttpContext.Session.SetString("email", email);
                return RedirectToAction("RecoverPass", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect Information");
            }
            return View("ForgetPass");
        }

        public IActionResult RecoverPass()
        {
            return View();
        }

        public async Task<IActionResult> _RecoverPass(string pass)
        {
            var user = await _userManager.FindByEmailAsync(HttpContext.Session.GetString("email"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, pass);

            if(result.Succeeded)
            {
                return RedirectToAction("Logins");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View("RecoverPass");
        }
    }
}
