using Microsoft.AspNetCore.Mvc;
using SATS.Core;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthorizationController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kullanıcı kimlik doğrulama (bu örnek için sabit bir kontrol yapılmıştır)
            if (request.Username == "1" && request.Password == "1")
            {
                var token = _tokenService.GenerateToken("1", request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }


    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
