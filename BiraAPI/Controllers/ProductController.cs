using BiraAPI.Data;
using BiraAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.Include(sh => sh.Category).ToListAsync();
            return Ok(products);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetSingleProduct(int id)
        {
            var product = await _context.Products
                .Include(h => h.Category)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> CreateProduct(Product product)
        {
            product.Category = null;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(await GetDbProducts());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Product product, int id)
        {
            var dbproduct = await _context.Products
                .Include(sh => sh.Category)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbproduct == null)
                return NotFound("Product not found.");

            dbproduct.Name = product.Name;
            dbproduct.Quantity = product.Quantity;
            dbproduct.Price = product.Price;
            dbproduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbProducts());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var dbProduct = await _context.Products
                .Include(sh => sh.Category)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbProduct == null)
                return NotFound("Product not found.");

            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();

            return Ok(await GetDbProducts());
        }

        private async Task<List<Product>> GetDbProducts()
        {
            return await _context.Products.Include(sh => sh.Category).ToListAsync();
        }
    }
}
