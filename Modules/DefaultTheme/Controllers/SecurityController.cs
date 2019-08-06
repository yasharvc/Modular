using Contracts.Authentication;
using Contracts.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DefaultTheme.Controllers
{
	[AuthenticationType(typeof(AnonymousAuthentication))]
	public class SecurityController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			var auth = new Authentication
			{
				HttpContext = HttpContext
			};
			if (auth.IsAuthenticated())
				return Redirect(new Authentication().LoginPagePath);
			return this.GetView();
		}

		[HttpPost]
		public IActionResult Login(string user, string pass)
		{
			if (new UserAuthenticator().Authenticate(HttpContext, user, pass))
				return Json(new { result = true });
			return Json(new { result = false, msg = "نام کاربری و یا رمز عبور اشتباه است" });
		}

		public IActionResult Logout()
		{
			new UserAuthenticator().Disprove(HttpContext);
			return Redirect($"/Security/{nameof(Login)}");
		}
	}
}
