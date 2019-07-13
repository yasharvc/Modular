using Microsoft.AspNetCore.Mvc;
using ModulesFileUploader;
using System.Collections.Generic;
using System.Linq;

namespace Modular.Controllers
{
	public class HomeController : Controller
    {
		public class C1
		{
			public string Name { get; set; }
			public int id { get; set; }
		}
        public IActionResult Index()
        {
            return View();
        }
		[HttpPost]
		public IActionResult Form(C1 test)
		{
			var request = HttpContext.Request;
			return Content($"Your name is {test.Name} with age {test.id}");
		}

		public IActionResult SimpleClass(C1 data)
		{
			return Content($"{data.Name}");
		}

		public IActionResult ListOfClass(List<C1> data)
		{
			return Content($"{string.Join(",", data.Select(m => $"{m.Name} - {m.id}"))}");
		}

		public IActionResult UploadWWW()
		{
			new FolderedFileUploader("__").Move("D:\\Test");
			return Content("DASDS");
		}
	}
}