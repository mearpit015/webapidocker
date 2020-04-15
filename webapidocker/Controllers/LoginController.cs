using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using webapidocker.Model;

namespace webapidocker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public  IActionResult Login()
        {
            string userName = "arpit010";
            string pass = "123";

            UserModel userModel = new UserModel()
            {
                UserName = userName,
                Password = pass
            };
            IActionResult response = Unauthorized();
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(pass))
            {
                var authenticateUser = AuthenticateUser(userModel);
                if (authenticateUser != null)
                {
                    var tokenstr = GenrateJWT(authenticateUser);
                   response = Ok(new { token = tokenstr });
                }
            }
            return response;
        }

        private UserModel AuthenticateUser(UserModel userModel)
        {
            UserModel user = null;

            if (userModel.UserName.Equals("arpit010") && userModel.Password.Equals("123"))
            {
                user = new UserModel { UserName = "arpit010", Password = "123", Email = "arpit@gmail.com" };
            }
            return user;
        }

        private string GenrateJWT(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,userModel.UserName),
               new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, userModel.Email),
               new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
               );
            var encryptToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encryptToken;
        }

        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;

            return "Welcome :" + userName;
        }

       [Authorize] 
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> GetValue()
        {
            return new string[] { "Value" };
        }

    }
}