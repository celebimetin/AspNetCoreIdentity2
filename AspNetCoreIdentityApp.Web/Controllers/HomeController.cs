using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid) { return View(); }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, Email = request.Email, PhoneNumber = request.Phone }, request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["successMessage"] = "Üyelik başarılı";
                return RedirectToAction(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre yanlıştır.");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamazsınız." });
                return View();
            }
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya Şifre yanlıştır.", $"Başarısız giriş sayısı = {await _userManager.GetAccessFailedCountAsync(hasUser)}" });

            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu mail adresine ait kullanıcı bulunamadı.");
                return View();
            }
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var resetPasswordLink = Url.Action("ResetPassword", "Home", new
            {
                userId = hasUser.Id,
                token = passwordResetToken
            }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmailLink(resetPasswordLink!, hasUser.Email!);

            TempData["successMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];
            if (userId == null || token == null)
            {
                throw new Exception("Bir hata oluştu");
            }

            var hasUser = await _userManager.FindByIdAsync(userId!.ToString());
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token!.ToString(), request.Password);
            if (result.Succeeded)
            {
                TempData["successMessage"] = "Şifreniz başarıyla güncellenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            }
            return View();
        }
    }
}