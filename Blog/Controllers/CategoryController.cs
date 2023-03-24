using blog.Data;
using blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] DataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();

                return Ok(categories);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "X001 - Não foi possivel obter as categorias");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "S001 - Falha interna no servidor");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, [FromServices] DataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "X002 - Não foi possivel obter a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "S002 - Falha interna no servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Category category, [FromServices] DataContext context)
        {
            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "X003 - Não foi possivel incluir a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "S003 - Falha interna no servidor");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] Category category, [FromServices] DataContext context)
        {
            try
            {
                var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

                existingCategory.Name = category.Name;
                existingCategory.Slug = category.Slug;

                context.Categories.Update(existingCategory);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "X004 - Não foi possivel alterar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "S004 - Falha interna no servidor");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id, [FromServices] DataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "X005 - Não foi possivel deletar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "S005 - Falha interna no servidor");
            }
        }
    }
}