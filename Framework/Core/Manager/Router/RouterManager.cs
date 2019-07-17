using Contracts;
using Contracts.Module;
using ControllerExecuter;
using CoreCommons;
using Manager.Module;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Manager.Router
{
	public class RouterManager
	{
		protected ControllerExecuter.ControllerExecuter ControllerExecuter { get; set; } = new ControllerExecuter.ControllerExecuter();
		protected List<RouteInformation> Routes { get; } = new List<RouteInformation>();

		public void ResolveRouteInformation(Assembly assembly)
		{
			var route = new ControllerExecuter.ControllerExecuter(assembly);
			ModuleResolver resolver = new ModuleResolver(assembly);
			var moduleName = resolver.GetModuleManifest().Name;
			Routes.RemoveAll(m => m.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
			ResolveRoutes(assembly, resolver.GetModuleManifest());
		}

		protected void ResolveRoutes(Assembly assembly, ModuleManifest moduleManifest)
		{
			var Types = assembly.GetTypes().Where(m => m.IsSubclassOf(typeof(Controller)));
			foreach (var type in Types)
			{
				MethodInfo[] methods = GetMethods(type);
				foreach (var method in methods)
					AddMethodToRoute(type, method, moduleManifest.Name);
			}
		}

		private MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.DeclaredOnly |
								   BindingFlags.Instance |
								   BindingFlags.Public);
		}

		private void AddMethodToRoute(Type type, MethodInfo method, string moduleName)
		{
			//TODO: Get Controller Methods
			var actionMethods = GetActionMethods(method);
			Routes.Add(new RouteInformation
			{
				AllowedMethods = actionMethods,
				Controller = type,
				Parameters = method.GetParameters().ToList(),
				Path = $"/{type.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)}/{method.Name}/",
				Prefix = $"",
				ModuleName = moduleName
			});
		}

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

		public RouteInformation GetRouteFor(string url)
		{
			if (!url.EndsWith('/'))
				url += '/';
			var res = Routes.Where(m => url.StartsWith(m.Path, StringComparison.OrdinalIgnoreCase)).OrderBy(m => m.Path.Length);
			return res.Last();
		}
	}
}
