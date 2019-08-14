using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modular.Classes;

namespace Modular.Areas._ModulesAdministration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : Controller
    {
		AdminAuthentication perm = new AdminAuthentication();
		public IActionResult Login(string error = "")
		{
			perm.HttpContext = HttpContext;
			if (perm.IsAuthenticated())
			{
				return Redirect($"/{nameof(_ModulesAdministration)}/");
			}
			return View(nameof(Login), error);
		}
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(string UserName, string Password)
		{
			perm.HttpContext = HttpContext;
			if (perm.IsAuthenticated())
				return Redirect($"/{nameof(_ModulesAdministration)}");
			var authenticated = perm.Authenticate(UserName, Password);
			if (authenticated)
				return Redirect($"/{nameof(_ModulesAdministration)}");
			return RedirectToAction(nameof(Login), new { error = "نام کاربری و یا رمز عبور اشتباه است" });
		}
		[AllowAnonymous]
		public IActionResult Logout()
		{
			perm.HttpContext = HttpContext;
			perm.Disprove();
			return Redirect(perm.LoginPagePath);
		}
	}
}