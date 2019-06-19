using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ControllerExecuter
{
	public class ControllerExecuter
	{
		Assembly Assembly { get; set; }
		public ControllerExecuter(Assembly assembly = null) => Assembly = assembly;
		List<RouteInformation> Routes { get; } = new List<RouteInformation>();

		public RouteInformation GetRouteFor(string url)
		{
			if (!url.EndsWith('/'))
				url += '/';
			var res = Routes.Where(m => url.StartsWith(m.Path, StringComparison.OrdinalIgnoreCase)).OrderBy(m => m.Path.Length);
			return res.Last();
		}

		public void ResolveRoutes()
		{
			var Types = Assembly.GetTypes().Where(m => m.IsSubclassOf(typeof(Controller)));
			Console.ForegroundColor = ConsoleColor.Yellow;
			foreach (var type in Assembly.GetTypes())
			{
				MethodInfo[] methods = GetMethods(type);
				foreach (var method in methods)
					AddMethodToRoute(type, method);
			}
		}

		private void AddMethodToRoute(Type type, MethodInfo method)
		{
			//TODO: Get Controller Methods
			Routes.Add(new RouteInformation
			{
				AllowedMethods = new List<Contracts.Enums.HttpMethod>
							{
								Contracts.Enums.HttpMethod.GET,
								Contracts.Enums.HttpMethod.POST
							},
				Controller = type,
				Parameters = method.GetParameters().ToList(),
				Path = $"/{type.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)}/{method.Name}/",
				Prefix = $""
			});
		}

		private static MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.DeclaredOnly |
										BindingFlags.Instance | BindingFlags.Public);
		}
	}
}
