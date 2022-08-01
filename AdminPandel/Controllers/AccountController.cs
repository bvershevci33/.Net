using AdminPandel.Models;
using AdminPandel.Services;
using AdminPandel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdminPandel.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IMailService mailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            MailService = mailService;
        }

        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public IMailService MailService { get; }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = await UserManager.FindByEmailAsync(model.Email);
                if (userIdentity != null 
                    && !userIdentity.EmailConfirmed 
                    && (await UserManager.CheckPasswordAsync(userIdentity, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Emaili nuk eshte konfirmuar akoma.");
                    return View(model);
                }

                var result = await SignInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Kredencialet nuk jane te sakta.");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (ModelState.IsValid)
            {

                var identityUser = new ApplicationUser()
                {
                    FristName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    BirthDay = model.BirthDay,
                    Email= model.Email,
                    UserName = model.Email,

                };

                IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    if (token == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email Konfirmim Tokeni nuk eshte gjenerauar.");
                        return View(model);
                    }
                    var baseUrl = "https://localhost:44370";
                    var confimrEmailUrs = Url.Action("ConfirmEmail", "Account",new { userId = identityUser.Id, token = token });
                    confimrEmailUrs = $"{baseUrl}/{confimrEmailUrs}";

                    // Send Email

                    var emailReques = new MailRequest();
                    emailReques.Subject = "PBCA: Konfirmim i Llogarise.";
                    emailReques.Body = confimrEmailUrs;
                    emailReques.ToEmail = identityUser.Email;

                    await MailService.SendEmailAsync(emailReques);
                 

                    await UserManager.AddToRoleAsync(identityUser, "Inspector");
                    //await SignInManager.SignInAsync(identityUser, false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token) 
        {
            if(userId == null  || token == null)
            {
                ModelState.AddModelError(string.Empty, "Id e perdoruesit ose Tokeni nuk jane valid.");
                return View();
            }
            var userIdentity = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.ConfirmEmailAsync(userIdentity, token);

            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmedEmail");
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
            return View();
        }


    }
}
