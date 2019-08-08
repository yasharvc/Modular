using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Controller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Modular.Areas._ModulesAdministration.Controllers
{
	[Area("_ModulesAdministration")]
	public class PanelController : Controller
    {
		public IActionResult Index() => View();

		public IActionResult Status()
		{
			var mdl = Startup.Manager.ModuleManager.GetModules().Select(m => m.Manifest).ToList();
			return View(mdl);
		}

		[HttpGet]
		public IActionResult NewToken() => View();

		[HttpGet]
		public JsonResult GetNewAuthToken() => Json(new { token = Startup.Manager.AuthenticationManager.GenerateNewToken() });
		[HttpGet]
		public JsonResult GetNewModuleToken() => Json(new { token = Startup.Manager.ModuleManager.GenerateNewToken() });
		[HttpGet]
		public IActionResult Upload()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Upload(IFormFile module)
		{
			var data = new byte[module.Length];
			module.OpenReadStream().Read(data, 0, (int)module.Length);
			Startup.Manager.Upload(data);
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult AllAuths()
		{
			return View(Startup.Manager.AuthenticationManager.GetInstalledAuthentications());
		}

		[HttpGet]
		public ActionResult GetAuthCode(string token)
		{
			var auth = Startup.Manager.AuthenticationManager.GetAuthenticationByToken(token);
			return this.ToFileResult(auth.GetCode(), "Authentication.cs");
		}

		[HttpGet]
		public ActionResult GetCode(string moduleName)
		{
			return this.ToFileResult(Startup.Manager.ModuleManager.GetModuleMeta(moduleName).ServiceMeta.ToString(), $"{moduleName}.cs");
		}
	}
}