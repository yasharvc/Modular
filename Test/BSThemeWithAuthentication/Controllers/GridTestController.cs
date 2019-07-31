using Microsoft.AspNetCore.Mvc;

namespace BSThemeWithAuthentication.Controllers
{
	public class GridTestController : Controller
	{
		public ContentResult Index() => Content("Test Grid");
	}
}
