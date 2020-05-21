using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServices.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> FindAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task AddAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .AllAsync(x => x.Category.Id != id);
        }
    }
}