using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreServices.Services;

namespace AspReact.Controllers.Admin
{
    [Route("admin/{controller}/{action}/{id?}")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Order
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetAsync());
        }

        // GET: Order/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.FindDetailed(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Order/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,FullPrice,IsPaid,IsDelivered")]
            Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddAsync(order);
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // GET: Order/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.FindAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,FullPrice,IsPaid,IsDelivered")]
            Order order)
        {
            if (id != order.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_orderService.OrderExists(order.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }
    }
}