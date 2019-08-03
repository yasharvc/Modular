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

		public ContentResult GetCode() => Content(Startup.Manager.ModuleManager.GetModuleCode("BTThemeWithAuth"));

		public string RunCode()
		{
			var meta = Startup.Manager.ModuleManager.GetModuleMeta("BTThemeWithAuth");
			var obj = meta.CreateObject("BSThemeWithAuthentication.SimpleService");
			return meta.InvokeMethod(obj, "GetName", "yashar") as string;
		}
	}
}
