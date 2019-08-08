using Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using Utility;

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
			return CallFunction("MachineWorkingInfo", "API.MachineWorkingInfo.Controllers.ValuesController", "GetMachine","[19]");
		}

		public string CallFunction(string ModuleName, string FullClassName, string ServiceName, string parameters)
		{
			try
			{
				var manager = Startup.Manager;//HttpContext.RequestServices.GetService(typeof(Manager)) as Manager;
				parameters = parameters.Replace("\\\"", "\"");
				parameters = parameters.Replace("\\\\\"", "\\\"");
				JsonHandler jsonHandler = new JsonHandler();
				var pars = jsonHandler.GetElementsInsideArray(parameters);
				return JsonConvert.SerializeObject(manager.ModuleManager.CallModuleFunction(ModuleName, FullClassName, ServiceName, pars.ToArray()));
			}catch(Exception e)
			{
				throw e;
			}
		}
	}
}
