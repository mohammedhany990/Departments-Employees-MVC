using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Linq;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSettings _mailSettings;

        public AccountController(UserManager<AppUser> userManager, 
                                 SignInManager<AppUser> signInManager,
                                 IEmailSettings mailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailSettings = mailSettings;
        }

        #region Registeration

        public IActionResult Registeration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registeration(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new AppUser() // Manual Mapping
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };

                var Result = await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (Flag)
                    {
                        var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Incorrect Password");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is not existing");
            }
            return View(model);
        }
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        #endregion

        #region SendEmail
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(User);

                    // To create the password link
                    var ResetPasswordLink = Url.Action("ResetPassword"/*ActionNAme*/,
                                                        "Account"/*ControllerName*/,
                                                        new { email = model.Email, Token=Token },
                                                        Request.Scheme);
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink,                        
                    };

                    _mailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckInbox));
                }
            }
            ModelState.AddModelError(string.Empty, "Email is not existing");
            return View(model);
        }

        public IActionResult CheckInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string Token)
        {
            TempData["email"] = email; ;
            TempData["token"] = Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                

                var User =await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        #endregion

        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResposne")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResposne()
        {
            var Result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var Claims = Result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            return RedirectToAction("Index", "Home");
        }
    }
}
