using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreServices.Services;

namespace AspReact.Controllers.Admin
{
    [Route("admin/{controller}/{action}/{id?}")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAsync());
        }

        // GET: Category/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _categoryService.FindAsync(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        // GET: Category/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _categoryService.FindAsync(id.Value);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_categoryService.CategoryExists(category.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _categoryService.FindAsync(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _categoryService.CanDeleteAsync(id))
            {
                return BadRequest("Can not delete category with products");
            }

            await _categoryService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}