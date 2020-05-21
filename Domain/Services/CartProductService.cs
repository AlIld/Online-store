using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServices.Services
{
    public class CartProductService
    {
        private readonly ApplicationDbContext _context;

        public CartProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<CartProduct>> GetCartProductsAsync(string userId)
        {
            return await _context.CartProducts
                .Include(x => x.Product)
                .Include(x => x.User)
                .Where(x =>
                    x.Product != null &&
                    x.User != null &&
                    x.User.Id == userId)
                .ToListAsync();
        }

        public async Task<CartProduct> GetCartProductAsync(int productId, string userId)
        {
            return await _context.CartProducts
                .Include(x => x.Product)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.Product != null &&
                    x.Product.Id == productId &&
                    x.User != null &&
                    x.User.Id == userId);
        }

        public async Task<CartProduct> AddProductAsync(int productId, string userId)
        {
            var cartProduct = await GetCartProductAsync(productId, userId);
            if (cartProduct != null)
            {
                cartProduct.Count++;
            }
            else
            {
                var user = await _context.Users.FindAsync(userId);
                var product = await _context.Products.FindAsync(productId);
                cartProduct = new CartProduct() {User = user, Product = product, Count = 1};
                _context.CartProducts.Add(cartProduct);
            }

            await _context.SaveChangesAsync();
            return cartProduct;
        }


        public async Task<CartProduct> RemoveProductAsync(int productId, string userId)
        {
            var cartProduct = await GetCartProductAsync(productId, userId);
            if (cartProduct != null)
            {
                if (cartProduct.Count > 1)
                {
                    cartProduct.Count--;
                }
                else
                {
                    _context.CartProducts.Remove(cartProduct);
                    cartProduct = null;
                }
            }
            else
            {
                return null;
            }

            await _context.SaveChangesAsync();
            return cartProduct;
        }

        public async Task DeleteProductAsync(int productId, string userId)
        {
            var cartProduct = await GetCartProductAsync(productId, userId);
            _context.CartProducts.Remove(cartProduct);
            await _context.SaveChangesAsync();
        }
    }
}