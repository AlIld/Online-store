using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreServices.Services;

namespace AspReact.Controllers.API
    {
        [Route("api/{controller}/{action}/{id?}")]
    public class CartProductController : Controller
    {
        private readonly CartProductService _cartProductService;

        public CartProductController(CartProductService cartProductService)
        {
            _cartProductService = cartProductService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return Ok(
                await _cartProductService.GetCartProductsAsync(User.FindFirstValue("sub")));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCartProduct(int productId)
        {
            return Ok(
                await _cartProductService.GetCartProductAsync(productId, User.FindFirstValue("sub")));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int productId)
        {
            return Ok(
                await _cartProductService.AddProductAsync(productId, User.FindFirstValue("sub")));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Remove(int productId)
        {
            return Ok(
                await _cartProductService.RemoveProductAsync(productId, User.FindFirstValue("sub")));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            await _cartProductService.DeleteProductAsync(productId, User.FindFirstValue("sub"));
            return Ok(null);
        }
    }
}