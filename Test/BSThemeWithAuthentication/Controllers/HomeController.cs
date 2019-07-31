using Microsoft.AspNetCore.Mvc;
using Contracts.Controller;

namespace BSThemeWithAuthentication.Controllers
{
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.GetView();
        }
    }
}