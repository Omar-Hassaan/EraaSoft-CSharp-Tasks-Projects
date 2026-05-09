using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MovieBookingApp.Models;
using MovieBookingApp.ViewModels;
using MovieBookingApp.Repositories;


namespace MovieBookingApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ApplicationUserOtp> _applicationUserOtpRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IRepository<ApplicationUserOtp> applicationUserOtpRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = applicationUserOtpRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Name = registerVM.Name,
                Address = registerVM.Address,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            TempData["Success-Notification"] = "User Registered Successfully";

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "Welcome to Movie Booking App",
                $"<h1>Hi {registerVM.Name},</h1><h2>Click <a href={link}>here</a> to confirm your email</h2><p>Thank you for registering at Movie Booking App. We're excited to have you on board!</p><p>Best regards,<br/>Movie Booking App Team</p>");
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                TempData["Error-Notification"] = "Invalid User";
                return RedirectToAction(nameof(Login));
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error-Notification"] = "Email Confirmation Failed";
                return RedirectToAction(nameof(Login));
            }
            TempData["Success-Notification"] = "Email Confirmed Successfully";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOrEmail)
                    ?? await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOrEmail);
            
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid User or Email");
                return View(resendEmailConfirmationVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Welcome to Movie Booking App",
                $"<h1>Hi {user.Name},</h1><h2>Click <a href={link}>here</a> to confirm your email</h2><p>Thank you for registering at Movie Booking App. We're excited to have you on board!</p><p>Best regards,<br/>Movie Booking App Team</p>");
            TempData["Success-Notification"] = "Resend Email Confirmation Successfully";
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail)
                ?? await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid User or Password");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is locked out. Please try again later.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Please Confirm Your Email First");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User or Password");
                }
                return View(loginVM);
            }
            TempData["Success-Notification"] = "Login Successful";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail)
                ?? await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid User or Password");
                return View(forgetPasswordVM);
            }

            var applicationUserotps = await _applicationUserOtpRepository.GetAsync(e => e.ApplicationUserId == user.Id);
            var count = applicationUserotps.Count(e => (DateTime.UtcNow - e.CreatedAt).TotalHours <= 24);
            //if (count >= 5)
            //{
            //    ModelState.AddModelError("", "Too many attempts. Please try again later.");
            //    return View(forgetPasswordVM);
            //}

            var otp = new Random().Next(1000, 9999).ToString();
            var applicationUserOtp = new ApplicationUserOtp(user.Id, otp);
            await _applicationUserOtpRepository.AddAsync(applicationUserOtp);
            await _applicationUserOtpRepository.CommitAsync();
            await _emailSender.SendEmailAsync(
                user.Email,
                "Password Reset OTP",
                $"<h1>Hi {user.Name},</h1><h2>Your OTP for password reset is: <span style=\"color: red\">{otp}</span></h2><p>If you did not request a password reset, please ignore this email.</p><p>Best regards,<br/>Movie Booking App Team</p>");
            return RedirectToAction(nameof(ValidateOTP), new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ValidateOTP(string userId)
        {
            return View(new ValidateOTPVM { UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM validateOTPVM)
        {
            if (!ModelState.IsValid)
            {
                return View(validateOTPVM);
            }
            var user = await _userManager.FindByIdAsync(validateOTPVM.UserId);

            if (user is null)
            {
                ModelState.AddModelError("", "Invalid User ");
                return View(validateOTPVM);
            }

            var otps = await _applicationUserOtpRepository.GetAsync(e =>
                e.ApplicationUserId == user.Id &&
                e.IsValid == true &&
                e.ValidTo >= DateTime.UtcNow
            );
            var otp = otps.OrderByDescending(e => e.CreatedAt).FirstOrDefault();
            if (otp is null || otp.OTP != validateOTPVM.OTP)
            {
                ModelState.AddModelError("", "Invalid or Expired OTP ");
                return View(validateOTPVM);
            }
            otp.IsValid = false;
            await _applicationUserOtpRepository.CommitAsync();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            TempData["token"] = token;
            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId)
        {
            var token = TempData["token"] as string;
            if (token is null)
            {
                return RedirectToAction(nameof(Login));
            }
            return View(new ResetPasswordVM() { UserId = userId, Token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (resetPasswordVM.Token is null)
            {
                return RedirectToAction(nameof(Login));
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid User ");
                return View(resetPasswordVM);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }
            return RedirectToAction(nameof(Login));
        }
    }
}
