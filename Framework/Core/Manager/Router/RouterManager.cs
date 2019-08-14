using Contracts;
using Contracts.Module;
using Manager.Module;
using Microsoft.AspNetCore.Mvc;
using RequestHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Manager.Router
{
	public class RouterManager
	{
		protected ControllerExecuter.ControllerExecuter ControllerExecuter { get; set; } = new ControllerExecuter.ControllerExecuter();
		protected List<RouteInformation> Routes { get; } = new List<RouteInformation>();
		protected Dictionary<string,Dictionary<string, string>> Redirections { get; } = new Dictionary<string,Dictionary<string, string>>();

		public void ResolveRouteInformation(Assembly assembly)
		{
			var route = new ControllerExecuter.ControllerExecuter(assembly);
			ModuleResolver resolver = new ModuleResolver(assembly);
			var moduleName = resolver.GetModuleManifest().Name;
			Routes.RemoveAll(m => m.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
			ResolveRoutes(assembly, resolver.GetModuleManifest());
			AddRedirections(resolver.GetModuleManifest());
		}

		private void AddRedirections(ModuleManifest moduleManifest) => Redirections[moduleManifest.Token] = moduleManifest.Redirections;

		public void AddRedirection(string moduleToken,string from, string to) => Redirections[moduleToken][from] = to;

		protected void ResolveRoutes(Assembly assembly, ModuleManifest moduleManifest)
		{
			var Types = assembly.GetTypes().Where(m => m.IsSubclassOf(typeof(Controller)));
			foreach (var type in Types)
			{
				var ctrlRouteAttr = type.GetCustomAttribute<RouteAttribute>();
				MethodInfo[] methods = GetMethods(type);
				foreach (var method in methods)
					AddMethodToRoute(type, method, moduleManifest.Name, ctrlRouteAttr);
			}
		}

		private MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.DeclaredOnly |
								   BindingFlags.Instance |
								   BindingFlags.Public);
		}

		private void AddMethodToRoute(Type type, MethodInfo method, string moduleName, RouteAttribute ctrlRouteAttr)
		{
			//TODO: Get Controller Methods
			var actionMethods = GetActionMethods(method);
			var prefix = $"/{type.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)}";
			var path = "";
			var routeActionName = method.Name;
			if(ctrlRouteAttr != null)
			{
				prefix = $"/{CompileTemplate(ctrlRouteAttr.Template,type)}";
			}
			var methodRouteAttr = method.GetCustomAttribute<RouteAttribute>();
			if(methodRouteAttr != null)
				routeActionName = CompileTemplate(methodRouteAttr.Template, type);
			path = $"{prefix}/{routeActionName}";
			try
			{
				prefix = path.Substring(0, path.IndexOf(type.Name.Replace("controller", "", StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase) - 1);
			}
			catch
			{
				prefix = "";
			}
			Routes.Add(new RouteInformation
			{
				AllowedMethods = actionMethods,
				Controller = type,
				Parameters = method.GetParameters().ToList(),
				Path = path,
				Prefix = prefix,
				ModuleName = moduleName,
				MethodName = method.Name,
				RouteActionName = routeActionName
			});
		}

		private string CompileTemplate(string template, Type type) => template.Replace("[controller]", type.Name.Replace("controller", "", StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase);

		private List<Contracts.Enums.HttpMethod> GetActionMethods(MethodInfo method)
		{
			var res = new List<Contracts.Enums.HttpMethod>();
			if (method.HasGetAttribute())
				res.Add(Contracts.Enums.HttpMethod.GET);
			if (method.HasPostAttribute())
				res.Add(Contracts.Enums.HttpMethod.POST);
			if (method.HasPutAttribute())
				res.Add(Contracts.Enums.HttpMethod.PUT);
			if (method.HasDeleteAttribute())
				res.Add(Contracts.Enums.HttpMethod.DELETE);
			if (res.Count == 0)
			{
				var values = Enum.GetValues(typeof(Contracts.Enums.HttpMethod));
				foreach (var item in values)
				{
					res.Add((Contracts.Enums.HttpMethod)item);
				}
			}
			return res;
		}

		public RouteInformation GetRouteFor(string url, RequestInformation requestInfo)
		{
			if (!url.EndsWith('/'))
				url += '/';
			List<RouteInformation> res = new List<RouteInformation>();
			foreach (var route in Routes)
			{
				if (url.StartsWith($"{route.Path}/", StringComparison.OrdinalIgnoreCase) && route.AllowedMethods.Contains(requestInfo.Method))
					res.Add(route);
			}
			//var res = Routes.Where(m => url.StartsWith(m.Path, StringComparison.OrdinalIgnoreCase) && m.AllowedMethods.Contains(requestInfo.Method)).OrderBy(m => m.Path.Length);
			if (res.Count == 0) {
				var dict = Redirections.Values;
				foreach (var item in dict)
				{
					foreach (KeyValuePair<string,string> keyValue in item)
					{
						if (keyValue.Key.Equals(url, StringComparison.OrdinalIgnoreCase))
						{
							return GetRouteFor(keyValue.Value, requestInfo);
						}
					}
				}
			}
			return res.Last();
		}
	}
}
