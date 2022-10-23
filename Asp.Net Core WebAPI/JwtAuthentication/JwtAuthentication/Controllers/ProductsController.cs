using JwtAuthentication.Data;
using JwtAuthentication.Dtos.ProductDtos;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin,Standard")]
    public class ProductsController : ControllerBase
    {

        private readonly AppDbContext context;

        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await context.Products.ToListAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product is null)
                return NotFound();
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] CreateProductDto product)
        {
            int isSaved = 0;
            if(ModelState.IsValid)
            {
                Product newProduct = new()
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                };
                await context.Products.AddAsync(newProduct);
                isSaved = await context.SaveChangesAsync();
                if (isSaved == 0) return Problem("Product could not created!");

                return CreatedAtAction("GetProduct", new { id = newProduct.Id }, newProduct);
            }
            ModelState.AddModelError("", "There is a problem!");
            return BadRequest();
            
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] UpdateProductDto product)
        {
            var existProduct = await context.Products.Where(p => p.Id == product.Id).SingleOrDefaultAsync();

            if (existProduct is null)
                return NotFound();

            existProduct.ProductName = product.ProductName;
            existProduct.Price = product.Price;
            existProduct.CategoryId = product.CategoryId;
            context.Products.Update(existProduct);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existProduct = await context.Products.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (existProduct is null)
                return NotFound();

            context.Products.Remove(existProduct);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
