using JwtAuthentication.Data;
using JwtAuthentication.Dtos.CategoryDtos;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext context;

        public CategoriesController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        //[EnableCors]   ====> cors politikalari gecerli
        public async Task<IActionResult> GetCategories()
        {
            var categories = await context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        //[DisableCors("CustomCorsPolicy")] ====> cors politikalari gecersiz
        public async Task<IActionResult> GetCategory(int? id)
        {
            if (id is null) return BadRequest();
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] CreateCategoryDto category)
        {
            int isSaved = 0;
            if (ModelState.IsValid)
            {
                Category newCategory = new()
                {
                     CategoryName = category.CategoryName
                };
                await context.Categories.AddAsync(newCategory);
                isSaved = await context.SaveChangesAsync();
                if (isSaved == 0) return Problem("Category could not created!");
                return CreatedAtAction("GetCategory", new {id = newCategory.Id}, newCategory);
            }
            ModelState.AddModelError(category.CategoryName, "Invalid");
            return BadRequest();

        }

        [HttpPut]
        public async Task<IActionResult> PutCategory([FromBody] UpdateCategoryDto category)
        {
            int isSaved = 0;
            var existCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existCategory is null) return NotFound();

            if (ModelState.IsValid)
            {
                existCategory.CategoryName = category.CategoryName;
                context.Categories.Update(existCategory);
                isSaved = await context.SaveChangesAsync();
                if (isSaved == 0) return Problem("Category could not updated!");
                return NoContent();
            }

            ModelState.AddModelError(category.CategoryName, "Invalid");
            return BadRequest();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            int isSaved = 0;
            if (id is null) return BadRequest();

            var existCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(existCategory is null) return NotFound();

            context.Categories.Remove(existCategory);
            isSaved = await context.SaveChangesAsync();

            if (isSaved == 0) return Problem("Category could not deleted!");

            return NoContent();
        }
    }
}
