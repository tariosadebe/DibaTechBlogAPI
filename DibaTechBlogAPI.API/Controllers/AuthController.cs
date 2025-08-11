using DibaTechBlogAPI.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DibaTechBlogAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // For demo: hardcoded company credentials
        private const string CompanyUsername = "company";
        private const string CompanyPassword = "password123";
        private const string JwtKey = "SuperSecretKeyForJwtTokenGeneration123!";
        private const string JwtIssuer = "DibaTechBlogAPI";

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            if (request.Username == CompanyUsername && request.Password == CompanyPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JwtKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, "Company")
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    Issuer = JwtIssuer,
                    Audience = JwtIssuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
            return Unauthorized();
        }
    }
}
