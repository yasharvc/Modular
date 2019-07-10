using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Modular.Areas._ModulesAdministration.Controllers
{
	[Area("_ModulesAdministration")]
	public class PanelController : Controller
    {
		public IActionResult Index() => View();

		[HttpGet]
		public IActionResult NewToken() => View();

		[HttpGet]
		public JsonResult GetNewToken() => Json(new { token = Startup.Manager.AuthenticationManager.GenerateNewToken() });

	}
}