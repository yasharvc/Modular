using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace AuthTest.Controllers
{
	public class SampleAuthController : Controller
	{
		[AuthenticationType(typeof(AnonymousAuthentication))]
		public IActionResult Index()
		{
			return Content("Inside Login");
		}
	}
}
