using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoAppIdentity.DTOs;
using TodoAppIdentity.Model;
using TodoAppIdentity.Services;

namespace TodoAppIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gera o token imediatamente após o registro
                var roles = await _userManager.GetRolesAsync(user);
                var tokenString = _tokenService.CreateToken(user, roles);

                return Ok(new TokenResponse
                {
                    Token = tokenString,
                    Expiration = DateTime.UtcNow.AddDays(7)
                });
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { Message = "Credenciais inválidas." });
            }

            // Geração do token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenString = _tokenService.CreateToken(user, roles);

            return Ok(new TokenResponse
            {
                Token = tokenString,
                Expiration = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
