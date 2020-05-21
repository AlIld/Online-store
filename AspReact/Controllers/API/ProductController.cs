using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using StoreServices.Services;

namespace AspReact.Controllers.API
{
    [Route("api/{controller}/{action}/{id?}")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _productService.GetAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ProductsForCategory(int categoryId)
        {
            return Ok(await _productService.GetForCategoryAsync(categoryId));
        }
    }
}