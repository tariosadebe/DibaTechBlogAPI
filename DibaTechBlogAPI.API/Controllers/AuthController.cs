using DibaTechBlogAPI.API.Models;
using DibaTechBlogAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(new { error = "Invalid email or password." });
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(new { error = "Invalid email or password." });
                }
                return Ok(new { token = GenerateJwtToken(user) });
            }
            catch (Exception ex)
            {
                // Log the error (in production, use a logger)
                return StatusCode(500, new { error = "An error occurred during login.", details = ex.Message });
            }
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
