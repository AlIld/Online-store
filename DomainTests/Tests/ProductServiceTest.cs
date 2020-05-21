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
    public class ProductServiceTest
    {
        [SetUp]
        public void Setup()
        {
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
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first"},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                CollectionAssert.AreEqual(
                    products,
                    await productService.GetAsync()
                );
            }
        }

        [Test]
        public async Task GetForCategoryAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                Assert.AreEqual(
                    1,
                    (await productService.GetForCategoryAsync(1)).Count
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
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                Assert.AreEqual(
                    1,
                    (await productService.FindAsync(1)).Id
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
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                await productService.AddAsync(new Product() {Id = 3, Name = "third"});
                Assert.AreEqual(
                    3,
                    (await productService.FindAsync(3)).Id
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
                var pr = new Product() {Id = 2, Name = "second"};
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    pr,
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                pr.Name = "third";
                await productService.UpdateAsync(pr);
                Assert.AreEqual(
                    "third",
                    (await productService.FindAsync(2)).Name
                );
            }
        }

        [Test]
        public async Task RemoveAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                await productService.RemoveAsync(2);
                Assert.AreEqual(
                    1,
                    (await productService.GetAsync()).Count
                );
            }
        }

        [Test]
        public async Task Exists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                Assert.IsTrue(
                    productService.Exists(2)
                );
            }
        }

        [Test]
        public async Task CanDeleteAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var products = new[]
                {
                    new Product() {Id = 1, Name = "first", Category = new Category {Id = 1}},
                    new Product() {Id = 2, Name = "second"},
                };

                context.Products.AddRange(products);
                context.SaveChanges();

                var productService = new ProductService(context);
                Assert.IsTrue(
                    await productService.CanDeleteAsync(2)
                );
            }
        }
    }
}