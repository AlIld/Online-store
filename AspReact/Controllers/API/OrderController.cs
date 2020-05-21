using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreServices;
using StoreServices.Services;

namespace AspReact.Controllers.API
{
    [Route("api/{controller}/{action}/{id?}")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _orderService.GetAsync(User.FindFirstValue("sub")));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            return Ok(await _orderService.GetAsync(orderId, User.FindFirstValue("sub")));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeOrder()
        {
            return Ok(await _orderService.MakeOrder(User.FindFirstValue("sub")));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Pay(int orderId, string address, [FromBody] CreditCardDto creditCard)
        {
            await _orderService.Pay(orderId, creditCard, address, User.FindFirstValue("sub"));
            return Ok(null);
        }
    }
}