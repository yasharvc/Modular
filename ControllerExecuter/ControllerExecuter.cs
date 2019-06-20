using Contracts;
using CoreCommons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RequestHandler;
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
			foreach (var type in Types)
			{
				MethodInfo[] methods = GetMethods(type);
				foreach (var method in methods)
					AddMethodToRoute(type, method);
			}
		}

		public ActionResult InvokeAction(RequestInformation requestInformation, HttpRequest request)
		{
			var routeData = GetRouteFor(request.Path);
			var tempParameters = requestInformation.RequestParameters;
			SortedDictionary<string, object> sortedParameter = new SortedDictionary<string, object>();
			var i = 0;
			foreach (var parameter in routeData.Parameters)
			{
				if (parameter.ParameterType.IsPrimitiveType())
				{
					var temp = tempParameters.Where(m => m.Name.Length == 0).OrderBy(m => m.Index);
					if(temp.Count() == 0)
					{ 
						temp = tempParameters.Where(m => m.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)).OrderBy(m => m.Name);
					}
					if(temp.Count() > 0)
						SetPrimitiveParameter(parameter.ParameterType, tempParameters, sortedParameter, i, temp);
				}
				i++;
			}
			return (ActionResult)routeData.Controller.GetMethod(routeData.GetActionName()).Invoke(Activator.CreateInstance(routeData.Controller), sortedParameter.Values.ToArray());
		}

		private void SetPrimitiveParameter(Type parameterType, List<RequestParameter> tempParameters, SortedDictionary<string, object> sortedParameter, int i, IOrderedEnumerable<RequestParameter> temp)
		{
			var first = temp.First();
			tempParameters.Remove(first);
			sortedParameter[i.ToString()] = Convert.ChangeType(first.Value, parameterType);
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

		private MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.DeclaredOnly |
								   BindingFlags.Instance     |
								   BindingFlags.Public);
		}
	}
}