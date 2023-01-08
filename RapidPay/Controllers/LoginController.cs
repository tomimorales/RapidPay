using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RapidPay.Data.Models;
using RapidPay.Helpers;

namespace RapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthenticationHelper _authHelper;

        public LoginController(IConfiguration config)
        {
            _authHelper = new AuthenticationHelper(config);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User login)
        {
            User user = _authHelper.AuthenticateUser(login);

            if (user == null)
                return Unauthorized();

            var tokenString = _authHelper.GenerateJWTToken(user);
            return Ok(new
            {
                token = tokenString
            });
        }
    }
}
