using System;
using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace TestWebApplication.Controllers
{
	[AuthenticationType(typeof(AnonymousAuthentication))]
    public class SecurityController : Controller
    {
        public IActionResult Login()
        {
			var referer = Request.Headers["Referer"];
			if (IsUrlRedirctable(referer))
				Response.Cookies.Append("ref", referer);
			return View();
        }

		[HttpPost]
		public IActionResult Login(string user, string pass)
		{
			var fromUrl = Request.Cookies["ref"];
			if (new TestAuth(HttpContext).Authenticate(user, pass) == false)
			{
				return Redirect("Login");
			}
			else
			{
				if (IsUrlRedirctable(fromUrl))
					return Redirect(fromUrl);
				else
					return Redirect("/");
			}
		}

		private bool IsUrlRedirctable(string fromUrl)
		{
			if (fromUrl.EndsWith("/Security/Login", StringComparison.OrdinalIgnoreCase) || fromUrl.EndsWith("/Security/Login/", StringComparison.OrdinalIgnoreCase))
				return false;
			return true;
		}
	}
}