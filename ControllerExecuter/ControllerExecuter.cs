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

		public ActionResult InvokeAction(RequestInformation requestInformation, RouteInformation routeData, HttpContext context)
		{
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
				var ctrl = Activator.CreateInstance(routeData.Controller) as Controller;
				ctrl.ControllerContext = new ControllerContext();
				ctrl.ControllerContext.HttpContext = context;
				return (ActionResult)routeData.Controller.GetMethod(routeData.GetActionName()).Invoke(ctrl, sortedParameter.Values.ToArray());
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

	}
}