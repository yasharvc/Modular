using Contracts;
using Microsoft.AspNetCore.Mvc;
using ModulesFileUploader.MVCFileUploader;
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
			HttpContext.Items[Consts.CONTEXT_ITEM_IS_IN_DEBUG] = "ASD";
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

		public IActionResult DebugTest()
		{
			return Content(((Modular.Classes.Configuration)HttpContext.RequestServices.GetService(typeof(Modular.Classes.Configuration))).DataBaseConnectionString());
		}

		public IActionResult UploadWWW()
		{
			Startup.Manager.Upload(System.IO.File.ReadAllBytes(@"D:\Test\Test.zip"));
			//new ViewsFileUploader("xyz","temp").Move("D:\\Test");
			//new PagesFileUploader("xyz","temp").Move("D:\\Test");
			return Content("DASDS");
		}
	}
}