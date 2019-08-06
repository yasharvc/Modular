using Contracts.Authentication;
using Contracts.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DefaultTheme.Controllers
{
	[AuthenticationType(typeof(Authentication))]
	public class HomeController : Controller
	{
		public IActionResult Index() => this.GetView();
	}
}