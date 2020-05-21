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
    public class CategoryServiceTest
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
                var categories = new[]
                {
                    new Category {Id = 1, Name = "first"},
                    new Category {Id = 2, Name = "second"},
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                CollectionAssert.AreEqual(
                    categories,
                    await categoryService.GetAsync()
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
                var categories = new[]
                {
                    new Category {Id = 1, Name = "first"},
                    new Category {Id = 2, Name = "second"},
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                Assert.AreEqual(
                    categories[0].Id,
                    (await categoryService.FindAsync(1)).Id
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
                var categories = new[]
                {
                    new Category {Id = 1, Name = "first"},
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                var category = new Category {Id = 2, Name = "second"};
                await categoryService.AddAsync(category);
                Assert.AreEqual(
                    category.Id,
                    (await categoryService.FindAsync(category.Id)).Id
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
                var category = new Category {Id = 1, Name = "first"};
                var categories = new[]
                {
                    category,
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                category.Name = "second";

                var categoryService = new CategoryService(context);
                await categoryService.UpdateAsync(category);
                Assert.AreEqual(
                    category.Name,
                    (await categoryService.FindAsync(category.Id)).Name
                );
            }
        }

        [Test]
        public async Task CategoryExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            IOptions<OperationalStoreOptions> someOptions = Options.Create(new OperationalStoreOptions());


            using (var context = new ApplicationDbContext(options, someOptions))
            {
                var category = new Category {Id = 1, Name = "first"};
                var categories = new[]
                {
                    category,
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                await categoryService.UpdateAsync(category);
                Assert.IsTrue(categoryService.CategoryExists(category.Id));
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
                var categories = new[]
                {
                    new Category {Id = 1, Name = "first"},
                    new Category {Id = 2, Name = "second"},
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                await categoryService.RemoveAsync(1);
                Assert.AreEqual(
                    1,
                    (await categoryService.GetAsync()).Count
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
                var categories = new[]
                {
                    new Category {Id = 1, Name = "first"},
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryService = new CategoryService(context);
                await categoryService.RemoveAsync(1);
                Assert.IsTrue(await categoryService.CanDeleteAsync(1));
            }
        }
    }
}