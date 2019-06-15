using Microsoft.AspNetCore.Mvc;

namespace Modular.Controllers
{
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}