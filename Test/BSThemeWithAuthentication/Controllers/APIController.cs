using Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BSThemeWithAuthentication.Controllers
{
	[AuthenticationType(typeof(AnonymousAuthentication))]
	public class APIController : Controller
    {
        public JsonResult Free()
        {
            return Json(new { name="yashar",family="aliabbasi",conn = Contracts.Hub.InvocationHub.GetConnectionString() });
        }

		[AuthenticationType(typeof(BTAuthentication))]
		public JsonResult auth()
		{
			return Json(new { name = "yashar", family = "aliabbasi", Authentication = nameof(BTAuthentication) });
		}

		public JsonResult DivByZero()
		{
			int i = 0;
			i = 100 / i;
			return Json(new { sdfsdf = "" });
		}

		
		public JsonResult ConnError()
		{
			SqlConnection connection = new SqlConnection("Data Source=192.168.0.56;Initial Catalog=CMMS_WEBasdfsdf;User ID=sa;Password=1111;");
			connection.Open();
			return Json(new { sdfsdf = "" });
		}
	}
}