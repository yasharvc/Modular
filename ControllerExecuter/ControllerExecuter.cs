using Contracts;
using Contracts.Exceptions.Application;
using Contracts.Exceptions.System;
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
			if (routeData.AllowedMethods.Contains(requestInformation.Method))
			{
				var tempParameters = requestInformation.RequestParameters;
				SortedDictionary<string, object> sortedParameter = new SortedDictionary<string, object>();
				var i = 0;
				foreach (var parameter in routeData.Parameters)
				{
					if (parameter.ParameterType.IsPrimitiveType())
						GetPrimitiveParameter(tempParameters, sortedParameter, i, parameter);
					else if (parameter.ParameterType == typeof(Guid))
						sortedParameter[i.ToString()] = new Guid(GetParameter(tempParameters, parameter).First().Value.ToString());
					else if (parameter.ParameterType.GetInterface(nameof(IFormFile)) != null)
						sortedParameter[i.ToString()] = tempParameters.Single(m => m.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)).File;
					else
						sortedParameter[i.ToString()] = GetComplexParameter(parameter, tempParameters);
					i++;
				}
				return (ActionResult)routeData.Controller.GetMethod(routeData.GetActionName()).Invoke(Activator.CreateInstance(routeData.Controller), sortedParameter.Values.ToArray());
			}
			throw new MethodNotAllowedException(routeData.GetControllerName(), routeData.GetActionName(), requestInformation.Method);
		}

		private object GetComplexParameter(ParameterInfo parameter, List<RequestParameter> tempParameters) =>
			parameter.CastToType(tempParameters);

		private void GetPrimitiveParameter(List<RequestParameter> tempParameters, SortedDictionary<string, object> sortedParameter, int i, ParameterInfo parameter)
		{
			IOrderedEnumerable<RequestParameter> temp = GetParameter(tempParameters, parameter);
			SetPrimitiveParameter(parameter.ParameterType, tempParameters, sortedParameter, i, temp);
		}

		private static IOrderedEnumerable<RequestParameter> GetParameter(List<RequestParameter> tempParameters, ParameterInfo parameter)
		{
			var temp = tempParameters.Where(m => m.Name.Length == 0).OrderBy(m => m.Index);
			if (temp.Count() == 0)
			{
				temp = tempParameters.Where(m => m.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)).OrderBy(m => m.Name);
			}
			if (temp.Count() > 0)
				return temp;
			throw new ParameterNotFoundException();
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
			var actionMethods = GetActionMethods(method);
			Routes.Add(new RouteInformation
			{
				AllowedMethods = actionMethods,
				Controller = type,
				Parameters = method.GetParameters().ToList(),
				Path = $"/{type.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)}/{method.Name}/",
				Prefix = $""
			});
		}

		private List<Contracts.Enums.HttpMethod> GetActionMethods(MethodInfo method)
		{
			var res = new List<Contracts.Enums.HttpMethod>();
			if (method.HasGetAttribute())
				res.Add(Contracts.Enums.HttpMethod.GET);
			if(method.HasPostAttribute())
				res.Add(Contracts.Enums.HttpMethod.POST);
			if (method.HasPutAttribute())
				res.Add(Contracts.Enums.HttpMethod.PUT);
			if (method.HasDeleteAttribute())
				res.Add(Contracts.Enums.HttpMethod.DELETE);
			if(res.Count == 0)
			{
				var values = Enum.GetValues(typeof(Contracts.Enums.HttpMethod));
				foreach (var item in values)
				{
					res.Add((Contracts.Enums.HttpMethod)item);
				}
			}
			return res;
		}

		private MethodInfo[] GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.DeclaredOnly |
								   BindingFlags.Instance     |
								   BindingFlags.Public);
		}
	}
}