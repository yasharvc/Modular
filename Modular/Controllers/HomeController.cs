using Microsoft.AspNetCore.Mvc;

namespace Modular.Controllers
{
	public class HomeController : Controller
    {
		public class C1
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Form(C1 test)
		{
			var request = HttpContext.Request;
			return Content($"Your name is {test.Name} with age {test.Age}");
		}
    }
}