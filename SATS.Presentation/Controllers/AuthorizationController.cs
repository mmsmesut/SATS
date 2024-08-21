using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SATS.Core;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Core.IEmailSender _emailSender;

        public AuthorizationController(TokenService tokenService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, Core.IEmailSender emailSender)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Kullanıcıyı username ile bul
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            // Kullanıcıyı doğrula
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }

            // Token oluştur
            var token = _tokenService.GenerateToken(user.Id, user.UserName);
            return Ok(new { Token = token });
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            // Yeni bir IdentityUser oluştur
            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            // Kullanıcıyı oluştur ve şifreyi belirle
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Kullanıcı başarılı bir şekilde oluşturuldu
                var token = _tokenService.GenerateToken(user.Id, user.UserName);
                return Ok(new { Token = token });
            }

            // Hata durumunda neden başarısız olduğunu döndür
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Ok("If your email address exists in our system, you will receive a password reset email.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token, email = request.Email }, Request.Scheme);

            var emailMessage = $"Please reset your password by clicking <a href='{resetUrl}'>here</a>.";
            await _emailSender.SendEmailAsync(user.Email, "Reset your password", emailMessage);

            return Ok("If your email address exists in our system, you will receive a password reset email.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Kullanıcı bulunamadıysa bile, güvenlik açısından genel bir mesaj döndürüyoruz.
                return Ok("Password reset process has been completed. If the email exists, the password is reset.");
            }

            // Şifre sıfırlama işlemini gerçekleştir
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Your password has been reset successfully.");
            }

            // Hata durumunda neden başarısız olduğunu döndür
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }



        [HttpPost("setup-two-factor")]
        public async Task<IActionResult> SetupTwoFactor([FromBody] TwoFactorSetupRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var setupCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var emailMessage = $"Your two-factor setup code is: {setupCode}";
            await _emailSender.SendEmailAsync(user.Email, "Two-Factor Authentication Setup", emailMessage);

            return Ok("Two-factor setup code sent to your email.");
        }

        [HttpPost("verify-two-factor")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Token);
            if (isValid)
            {
                return Ok("Two-factor authentication verified successfully.");
            }

            return BadRequest("Invalid two-factor authentication code.");
        }
    }


    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignUpRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class TwoFactorRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class TwoFactorSetupRequest
    {
        public string Email { get; set; }
    }
}
