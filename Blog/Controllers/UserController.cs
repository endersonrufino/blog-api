using blog.Data;
using blog.Extensions;
using blog.Models;
using blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace blog.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel user, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));
                }

                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Slug = user.Email.Replace("@", "-").Replace(".", "-"),
                    Image = "image.png"
                };

                var password = PasswordGenerator.Generate(25);

                newUser.PasswordHash = PasswordHasher.Hash(password);

                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(400, new ResultViewModel<User>("S001 - Este e-mail já esta cadastrado" + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("S001 - Falha interna no servidor"));
            }
        }
    }
}