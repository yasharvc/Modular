using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BSThemeWithAuthentication.Controllers
{
	[AuthenticationType(typeof(AnonymousAuthentication))]
    public class SecurityController : Controller
    {
		[HttpGet]
        public IActionResult Login()
        {
			var auth = new BTAuthentication
			{
				HttpContext = HttpContext
			};
			if (auth.IsAuthenticated())
				return Redirect("/Home/Index");
            return View();
        }

		[HttpPost]
		public IActionResult Login(string user, string pass)
		{
			if (user == "940007" && pass == "123")
			{
				HttpContext.Response.Cookies.Append("FX", "");
				return Json(new { result = true });
			}
			return Json(new { result = false, msg = "نام کاربری و یا رمز عبور اشتباه است" });
		}

		public IActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete("FX");
			return Redirect($"/Security/{nameof(Login)}");
		}
    }
}