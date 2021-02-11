using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Autentykacja.DTO;
using Autentykacja.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Autentykacja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
         private readonly IConfiguration config;
        public AuthController(IUserService userService_, IConfiguration config_)
        {
            userService = userService_;
            config = config_;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(userService.GetUser("name1", "password1")); 
        }

        [HttpPost("login")]    
        public IActionResult Login(UserForLoginDTO user){
        
        var userFromService = userService.GetUser(user.Username,user.Password);
        
            if (userFromService == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromService.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromService.Username),
                new Claim(ClaimTypes.Role, userFromService.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
           
        }

    }
}