using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServices.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<List<Product>> GetForCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(x => x.Category)
                .Where(x => x.Category.Id == categoryId)
                .ToListAsync();
        }

        public async Task<Product> FindAsync(int id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return await _context.OrderProducts
                       .Include(x => x.Product)
                       .AllAsync(x => x.Product.Id != id) &&
                   await _context.CartProducts
                       .Include(x => x.Product)
                       .AllAsync(x => x.Product.Id != id);
        }
    }
}