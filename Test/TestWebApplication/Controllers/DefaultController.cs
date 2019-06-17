using Microsoft.AspNetCore.Mvc;

namespace TestWebApplication.Controllers
{
	public class DefaultController : Controller
	{
		public class Test
		{
			public string Name { get; set; }
			public string Age { get; set; }
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Form(Test test)
		{
			return Content($"Age {test.Name} - {test.Age}");
		}
	}
}