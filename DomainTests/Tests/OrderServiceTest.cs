using System;
using System.Threading.Tasks;
using Data;
using Data.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using StoreServices.Services;

namespace DomainTests
{
    public class OrderServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAsync1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1, User = new User() {Id = "1"}},
                    new Order {Id = 2, FullPrice = 2},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                Assert.AreEqual(
                    1,
                    (await orderService.GetAsync("1")).Count
                );
            }
        }

        [Test]
        public async Task FindAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                    new Order {Id = 2, FullPrice = 2},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                Assert.AreEqual(
                    orders[0].Id,
                    (await orderService.FindAsync(orders[0].Id)).Id
                );
            }
        }

        [Test]
        public async Task UpdateAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var order = new Order {Id = 1, FullPrice = 1};
                var orders = new[]
                {
                    order,
                    new Order {Id = 2, FullPrice = 2},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();


                var orderService = new OrderService(context);
                order.FullPrice = 10;
                await orderService.UpdateAsync(order);
                Assert.AreEqual(
                    10,
                    (await orderService.FindAsync(orders[0].Id)).FullPrice
                );
            }
        }

        [Test]
        public async Task OrderExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                    new Order {Id = 2, FullPrice = 2},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                Assert.IsTrue(orderService.OrderExists(orders[0].Id));
            }
        }


        [Test]
        public async Task FindDetailed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                    new Order {Id = 2, FullPrice = 2},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                Assert.AreEqual(
                    orders[0].Id,
                    (await orderService.FindDetailed(orders[0].Id)).Id
                );
            }
        }


        [Test]
        public async Task AddAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                await orderService.AddAsync(new Order {Id = 2, FullPrice = 2});
                Assert.AreEqual(
                    2,
                    (await orderService.GetAsync()).Count
                );
            }
        }

        [Test]
        public async Task MakeOrder()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                };

                var user = new User() {Id = "user1"};

                context.Orders.AddRange(orders);
                context.Users.Add(user);
                context.CartProducts.Add(new CartProduct()
                {
                    User = user,
                    Product = new Product {Price = 10},
                    Count = 2
                });
                context.SaveChanges();

                var orderService = new OrderService(context);
                await orderService.MakeOrder("user1");
                Assert.AreEqual(
                    2,
                    (await orderService.GetAsync()).Count
                );
            }
        }


        [Test]
        public async Task GetAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                await orderService.AddAsync(new Order {Id = 2, FullPrice = 2});
                Assert.AreEqual(
                    2,
                    (await orderService.GetAsync()).Count
                );
            }
        }

        [Test]
        public async Task GetAsync2()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var orders = new[]
                {
                    new Order {Id = 1, FullPrice = 1, User = new User{Id = "1"}},
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();

                var orderService = new OrderService(context);
                Assert.AreEqual(
                    orders[0],
                    (await orderService.GetAsync(1,"1"))
                );
            }
        }

        [Test]
        public async Task Pay()
        {
            Assert.Pass();
        }

        //
    }
}