using System.Text.RegularExpressions;
using blog.Data;
using blog.Extensions;
using blog.Models;
using blog.Services;
using blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Post(
            [FromBody] CreateUserViewModel user,
            [FromServices] EmailService emailService,
            [FromServices] DataContext context)
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

                emailService.Send(
                    user.Name,
                    user.Email,
                    "Bem vindo ao blog!",
                    $"Sua senha é {password}");

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

        [Authorize]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel uploadImage,
            [FromServices] DataContext context
        )
        {
            try
            {
                var filename = $"{Guid.NewGuid().ToString()}.jpg";
                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(uploadImage.Base64Image, "");

                var bytes = Convert.FromBase64String(data);

                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{filename}", bytes);

                var user = await context
                .Users
                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                if (user == null)
                {
                    return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
                }

                user.Image = filename;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Imagem alterada com sucesso"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("S001 - Falha interna no servidor"));
            }
        }
    }
}