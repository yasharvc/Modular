using Contracts;
using Contracts.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
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
				HttpContext.Items[Consts.CONTEXT_ITEM_IS_IN_DEBUG] = true;
				return JsonConvert.SerializeObject(manager.ModuleManager.CallModuleFunction(ModuleName, FullClassName, ServiceName, pars.ToArray()));
			}catch(Exception e)
			{
				throw e;
			}
		}

		[HttpPost]
		public string GetUsers()
		{
			HttpContext.Items[Consts.CONTEXT_ITEM_IS_IN_DEBUG] = true;
			var conf = HttpContext.RequestServices.GetService(typeof(Classes.Configuration)) as Classes.Configuration;
			var connStr = conf.DataBaseConnectionString();
			var conn = new SqlConnection(connStr);
			var users = conn.Query<User>("SELECT ID,UserName,FullName FROM dbo.Users WHERE IsActive = 1");
			return JsonConvert.SerializeObject(users);
		}
	}
}
