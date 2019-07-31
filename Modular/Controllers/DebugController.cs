using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Modular.Controllers
{
	public class DebugController : Controller
	{
		public ContentResult ConnectionString()
		{
			HttpContext.Items[Consts.CONTEXT_ITEM_IS_IN_DEBUG] = true;
			return Content(Contracts.Hub.InvocationHub.GetConnectionString());
		}
	}
}
