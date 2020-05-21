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
    public class CartProductServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetCartProductsAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var users = new[]
                {
                    new User {Id = "user1"},
                };
                var cartProducts = new[]
                {
                    new CartProduct {Id = 1, User = users[0], Product = new Product()},
                };

                context.Users.AddRange(users);
                context.CartProducts.AddRange(cartProducts);
                context.SaveChanges();

                var cartProductService = new CartProductService(context);
                CollectionAssert.AreEqual(
                    cartProducts,
                    await cartProductService.GetCartProductsAsync("user1")
                );
            }
        }

        [Test]
        public async Task GetCartProductAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var users = new[]
                {
                    new User {Id = "user1"},
                };
                var cartProducts = new[]
                {
                    new CartProduct {Id = 1, User = users[0], Product = new Product()},
                };

                context.Users.AddRange(users);
                context.CartProducts.AddRange(cartProducts);
                context.SaveChanges();

                var cartProductService = new CartProductService(context);
                Assert.AreSame(
                    cartProducts[0],
                    await cartProductService.GetCartProductAsync(1, "user1")
                );
            }
        }

        [Test]
        public async Task AddProductAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var product = new Product();
                var users = new[]
                {
                    new User {Id = "user1"},
                };
                var cartProducts = new[]
                {
                    new CartProduct {Id = 1, Count = 4, User = users[0], Product = product},
                };

                context.Users.AddRange(users);
                context.CartProducts.AddRange(cartProducts);
                context.SaveChanges();

                var cartProductService = new CartProductService(context);
                Assert.AreEqual(
                    5,
                    (await cartProductService.AddProductAsync(1, "user1")).Count
                );
            }
        }

        [Test]
        public async Task RemoveProductAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var product = new Product();
                var users = new[]
                {
                    new User {Id = "user1"},
                };
                var cartProducts = new[]
                {
                    new CartProduct {Id = 1, Count = 4, User = users[0], Product = product},
                };

                context.Users.AddRange(users);
                context.CartProducts.AddRange(cartProducts);
                context.SaveChanges();

                var cartProductService = new CartProductService(context);
                Assert.AreEqual(
                    3,
                    (await cartProductService.RemoveProductAsync(1, "user1")).Count
                );
            }
        }

        [Test]
        public async Task DeleteProductAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var product = new Product();
                var users = new[]
                {
                    new User {Id = "user1"},
                };
                var cartProducts = new[]
                {
                    new CartProduct {Id = 1, Count = 4, User = users[0], Product = product},
                };

                context.Users.AddRange(users);
                context.CartProducts.AddRange(cartProducts);
                context.SaveChanges();

                var cartProductService = new CartProductService(context);
                await cartProductService.DeleteProductAsync(1, "user1");
                Assert.Null(await cartProductService.GetCartProductAsync(1, "user1"));
            }
        }
    }
}