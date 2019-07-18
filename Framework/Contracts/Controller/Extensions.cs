using Contracts.Exceptions.System;
using Contracts.Hub;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Contracts.Controller
{
	public static class Extensions
	{
		public static ViewResult GetView(this Microsoft.AspNetCore.Mvc.Controller ctrl, [CallerMemberName] string name = "", object model = null)
		{
			name = GetLastMethodName(name);
			if (InvocationHub.IsModuleInDebugMode())
				return ctrl.View(name, model);
			else
			{
				var mdlName = ctrl.ControllerContext.HttpContext.Items[Consts.CONTEXT_ITEM_KEY_THEME_MODULE_NAME].ToString();
				var controllerName = ctrl.GetType().Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
				var path = $"{Consts.MODULES_BASE_PATH}\\{mdlName}\\Views\\{controllerName}\\";
				var file = "";
				try
				{
					file = Directory.GetFiles(path, $"{name}.*").Single();
				}
				catch (FileNotFoundException)
				{
					throw new ViewFileNotFoundException(mdlName, controllerName, name);
				}
				return ctrl.View($"~/{path.Replace("\\","/")}{new FileInfo(file).Name}", model);
			}
		}

		private static string GetLastMethodName(string name = "")
		{
			if (name == null)
			{
				int i = 1;
				while ((name = new System.Diagnostics.StackFrame(i++, true).GetMethod().Name).Equals("view", StringComparison.OrdinalIgnoreCase)) ;
			}
			return name;
		}
	}
}