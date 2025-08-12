
using DibaTechBlogAPI.API.Models;
using DibaTechBlogAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DibaTechBlogAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

    [HttpPost("register")]
    // To restrict registration to admins only, uncomment the line below after seeding an admin user:
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "A user with this email already exists." });
            }
            var user = new AppUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            return BadRequest(result.Errors);
        }

        private string GenerateJwtToken(AppUser user)
        {
            var jwtKey = _config["JWT_KEY"] ?? "SuperSecretKeyForJwtTokenGeneration123!";
            var jwtIssuer = _config["JWT_ISSUER"] ?? "DibaTechBlogAPI";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email ?? user.UserName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = jwtIssuer,
                Audience = jwtIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
