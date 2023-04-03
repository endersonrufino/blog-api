using blog.Data;
using blog.Extensions;
using blog.Services;
using blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public AccountController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("v1/login")]
        public async Task<IActionResult> Login(
            [FromServices] TokenService tokenService,
            [FromServices] DataContext context,
            [FromBody] AuthenticateViewModel authenticate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
                }

                var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == authenticate.Email);

                if (user == null)
                {
                    return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));
                }

                if (!PasswordHasher.Verify(user.PasswordHash, authenticate.Password))
                {
                    return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));
                }

                var token = tokenService.GenerateToken(user);

                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("S001 - Falha interna no servidor"));
            }
        }
    }
}