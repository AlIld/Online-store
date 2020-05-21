using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspReact.Controllers.Admin
{
    [Route("admin/{controller}/{action}/{id?}")]
    public class HomeController : Controller
    {
        // GET
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}