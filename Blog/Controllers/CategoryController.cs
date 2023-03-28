using blog.Data;
using blog.Extensions;
using blog.Models;
using blog.ViewModels;
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

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("X001 - Não foi possivel obter as categorias"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("S001 - Falha interna no servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, [FromServices] DataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));
                }

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("X002 - Não foi possivel obter a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("S001 - Falha interna no servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateCategoryViewModel category, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }
            try
            {
                var newCategory = new Category
                {
                    Name = category.Name,
                    Slug = category.Slug
                };

                await context.Categories.AddAsync(newCategory);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{newCategory.Id}", new ResultViewModel<Category>(newCategory));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("X003 - Não foi possivel incluir a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("S003 - Falha interna no servidor"));
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] EditCategoryViewModel category, [FromServices] DataContext context)
        {
            try
            {
                var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

                if (existingCategory == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));
                }

                existingCategory.Name = category.Name;
                existingCategory.Slug = category.Slug;

                context.Categories.Update(existingCategory);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(existingCategory));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("X004 - Não foi possivel alterar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("S004 - Falha interna no servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id, [FromServices] DataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("X005 - Não foi possivel deletar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("S005 - Falha interna no servidor"));
            }
        }
    }
}