using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserModel userModelObj)
        {
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(userModelObj);
            if (user != null) 
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new {token=tokenString});
            }

            return response;
        }

        [HttpGet("GetUserClaimInfo")]
        [Authorize]
        public ActionResult<IEnumerable<string>> GetUserClaimInfo()
        {
            var currentUser = HttpContext.User;
            return new string[] { (currentUser.HasClaim(x => x.Type == "UserName")).ToString(), (currentUser.HasClaim(x => x.Type == "Email")).ToString() };
        }
        private UserModel AuthenticateUser(UserModel login) 
        {
            UserModel user = null;
            if (login.UserName == "Kuldeep") 
            {
                user = new UserModel { UserName = "Kuldeep Singh Rathore", Email = "rathore.kuldeep761@gmail.com" };
            }
            return user;
        }
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
