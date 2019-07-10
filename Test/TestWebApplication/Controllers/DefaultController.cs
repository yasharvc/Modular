using Contracts.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestWebApplication.Controllers
{
	public class DefaultController : FCOntroller
	{
		public class Test
		{
			public string Name { get; set; }
			public string id { get; set; }
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Form(Test test)
		{
			return Content($"Age {test.Name} - {test.id}");
		}

		public IActionResult Zest()
		{
			return Content($"My name is yashar");
		}
		[HttpPut]
		public IActionResult ZestOfBitchi()
		{
			return Content($"My name is Zest of bitchi");
		}

		[HttpPut]
		[HttpPost]
		[AuthenticationType(typeof(Dummy2Authentication))]
		public IActionResult GetInfo(int id, string name)
		{
			return Content($"My name is {name} and my id is {id}");
		}

		public IActionResult GuidTest(Guid guid) => Content($"guid was {guid.ToString()}");

		public IActionResult SimpleClass(Test data)
		{
			return Content($"{data.Name}");
		}

		public IActionResult ListOfClass(List<Test> data)
		{
			return Content($"{string.Join(",",data.Select(m => $"{m.Name} - {m.id}"))}");
		}

		public IActionResult ListOfStrings(List<string> data)
		{
			return Content($"{string.Join(",", data)}");
		}
		[HttpPost]
		public IActionResult FileTest(string name, IFormFile file)
		{
			return Content($"{file.FileName}");
		}
	}

	public class FCOntroller : Controller
	{
		public int x { get; set; }
	}
}