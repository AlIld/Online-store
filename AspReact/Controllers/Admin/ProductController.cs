using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreServices.Services;

namespace AspReact.Controllers.Admin
{
    [Route("admin/{controller}/{action}/{id?}")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Product
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAsync());
        }

        // GET: Product/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.FindAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        private async Task<SelectList> GetCategoriesSelectList()
        {
            return new SelectList(await _categoryService.GetAsync(), "Id", "Name");
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategoriesSelectList();
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product product, int categoryId)
        {
            product.Category = await _categoryService.FindAsync(categoryId);
            if (product.Category == null)
            {
                ModelState.AddModelError("Category", "Category is not found");
            }

            if (ModelState.IsValid)
            {
                await _productService.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await GetCategoriesSelectList();
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.FindAsync(id.Value);
            if (product == null) return NotFound();
            
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageSrc")]
            Product product, int categoryId)
        {
            product.Category = await _categoryService.FindAsync(categoryId);
            if (product.Category == null)
            {
                ModelState.AddModelError("Category", "Category is not found");
            }
            
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productService.Exists(product.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await GetCategoriesSelectList();
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.FindAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _productService.CanDeleteAsync(id))
            {
                return BadRequest("Can not delete product that is in any user order or cart");
            }

            await _productService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}