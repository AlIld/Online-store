using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreServices.Services;

namespace AspReact.Controllers.API
{
    [Route("api/{controller}/{action}/{id?}")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _categoryService.GetAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            return Ok(await _categoryService.FindAsync(categoryId));
        }
    }
}