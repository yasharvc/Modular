using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BSThemeWithAuthentication.Controllers
{
	public class APIController : Controller
    {
		[AuthenticationType(typeof(AnonymousAuthentication))]
        public JsonResult Free()
        {
            return Json(new { name="yashar",family="aliabbasi" });
        }

		[AuthenticationType(typeof(BTAuthentication))]
		public JsonResult auth()
		{
			return Json(new { name = "yashar", family = "aliabbasi", Authentication = nameof(BTAuthentication) });
		}
    }
}