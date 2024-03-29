﻿using Contracts;
using Contracts.Controller;
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

		public object InvokeAction(RequestInformation requestInformation, RouteInformation routeData, HttpContext context)
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
				ctrl.ControllerContext = new ControllerContext
				{
					HttpContext = context
				};
				try
				{
					return GetMethod(routeData, requestInformation).Invoke(ctrl, sortedParameter.Values.ToArray());
				}
				catch (Exception e)
				{
					throw new MethodRuntimeException(e.InnerException != null ? e.InnerException : e);
				}
			}
			throw new MethodNotAllowedException(routeData.GetControllerName(), routeData.MethodName, requestInformation.Method);
		}

		private MethodInfo GetMethod(RouteInformation routeData, RequestInformation requestInformation) => routeData.Controller.GetMethods().Single(m => m.Name.Equals(routeData.MethodName, StringComparison.OrdinalIgnoreCase) && m.CheckMethod(requestInformation.Method));

		private object GetComplexParameter(ParameterInfo parameter, List<RequestParameter> tempParameters)
		{
			if(new TypeConverter.TypeConverter().IsList(parameter.ParameterType))
			{
				var converter = new TypeConverter.TypeConverter();
				var IListRef = typeof(IList<>);
				Type[] IListParam = { parameter.ParameterType.GetGenericArguments()[0] };
				object res = converter.CreateObjectByType(parameter.ParameterType);
				
				var props = parameter.ParameterType.GetGenericArguments()[0].GetProperties();
				var indices = tempParameters.Select(m => int.Parse(m.Index)).Distinct();
				foreach (var index in indices)
				{
					var parameters = tempParameters.Where(m => m.Name == parameter.Name && m.Index == index.ToString()).Select(m => new { m.PropertyName, m.Value });
					var obj = Activator.CreateInstance(parameter.ParameterType.GetGenericArguments()[0]);
					foreach (var prop in props)
					{
						var value = parameters.FirstOrDefault(m => m.PropertyName.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
						prop.SetValue(obj, converter.Convert(value?.Value,prop.PropertyType) );
					}
					res.GetType().GetMethod("Add").Invoke(res, new object[] { obj });
				}
				return res;
			}
			else
				return parameter.CastToType(tempParameters);
		}

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