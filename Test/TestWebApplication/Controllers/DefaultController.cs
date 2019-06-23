﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace TestWebApplication.Controllers
{
	public class DefaultController : FCOntroller
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
		public IActionResult GetInfo(int id, string name)
		{
			return Content($"My name is {name} and my id is {id}");
		}

		public IActionResult GuidTest(Guid guid) => Content($"guid was {guid.ToString()}");
	}

	public class FCOntroller : Controller
	{
		public int x { get; set; }
	}
}