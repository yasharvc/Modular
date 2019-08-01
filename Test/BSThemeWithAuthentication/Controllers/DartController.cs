using Microsoft.AspNetCore.Mvc;

namespace BSThemeWithAuthentication.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class DartController : Controller
    {
		[Route("index")]
		[HttpGet("{id}")]
		public string GetMachine(int id)
		{
			return $"GetMachine : {id}";
		}
	}
}