using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServices.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> FindAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
        
        public bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public async Task<Order> FindDetailed(int id)
        {
            return await _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> MakeOrder(string userId)
        {
            var cartProducts = await _context.CartProducts
                .Include(x => x.Product)
                .Include(x => x.User)
                .Where(x =>
                    x.User != null &&
                    x.User.Id == userId &&
                    x.Product != null)
                .ToListAsync();

            var order = new Order()
            {
                User = await _context.Users.FindAsync(userId),
                DateTime = DateTime.Now,
            };

            order.OrderProducts = cartProducts
                .Select(x => new OrderProduct() {Count = x.Count, Product = x.Product})
                .ToList();
            order.FullPrice = order.OrderProducts
                .Select(x => x.Product.Price * x.Count)
                .Aggregate((a, b) => a + b);

            await _context.Orders.AddAsync(order);
            _context.CartProducts.RemoveRange(cartProducts);

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAsync(string userId)
        {
            return await _context.Orders
                .Include(x => x.User)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .Where(x => x.User != null && x.User.Id == userId)
                .ToListAsync();
        }

        public async Task<Order> GetAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(x => x.User)
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.User != null && x.User.Id == userId &&
                    x.Id == orderId);
        }

        public async Task Pay(int orderId,
            CreditCardDto creditCard,
            string address,
            string userId)
        {
            if (await ValidatePayment(creditCard))
            {
                (await _context.Orders
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x =>
                        x.User != null && x.User.Id == userId &&
                        x.Id == orderId)).IsPaid = true;
                Console.WriteLine($"Address is {address}");
            }

            await _context.SaveChangesAsync();
        }

        private async Task<bool> ValidatePayment(CreditCardDto creditCard)
        {
            Console.WriteLine($"Number: {creditCard.Number}," +
                              $"Name: {creditCard.Name}," +
                              $"Expiry: {creditCard.Expiry}," +
                              $"CVC: {creditCard.Cvc}");
            await Task.Delay(2000);
            return true;
        }
    }
}