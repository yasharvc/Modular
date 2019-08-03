using Contracts.Authentication;
using Contracts.Controller;
using Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contracts
{
	public class RouteInformation
	{
		public string Prefix { get; set; }
		public string Path { get; set; }
		public List<HttpMethod> AllowedMethods { get; set; } = new List<HttpMethod>();
		public Type Controller { get; set; }
		public List<ParameterInfo> Parameters { get; set; } = new List<ParameterInfo>();
		public string MethodName { get; set; } = "";
		public string RouteActionName { get; set; } = "";

		public string ModuleName { get; set; }

		public string GetControllerName() => Controller.Name.Replace("controller","",StringComparison.OrdinalIgnoreCase);// Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
		public string GetActionName() => RouteActionName;// Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];

		public string GetQueryString(string url)
		{
			var haveFirstSlash = url.StartsWith("/");
			var haveLastSlash = url.EndsWith('/');
			var fixedURL = url;
			if (!haveFirstSlash)
			{
				fixedURL = $"/{fixedURL}";
			}
			if (!haveLastSlash)
			{
				fixedURL = $"{fixedURL}/";
			}
			string path = $"{(Prefix.Length > 0 ? Prefix + (Prefix.EndsWith("/") ? "" : "/") : "/")}{GetControllerName()}/{GetActionName()}/";
			if (fixedURL.StartsWith(path, StringComparison.OrdinalIgnoreCase))
			{
				return url.Substring(path.Length - (!haveFirstSlash ? 1 : 0) - (!haveLastSlash ? 1 : 0));
			}
			else
			{
				return url;
			}
		}

		public IAuthenticationType GetAuthentcationType(HttpMethod httpMethod)
		{
			var res = new AnonymousAuthentication();

			try
			{
				var method = Controller.GetMethods().SingleOrDefault(m => m.Name.Equals(MethodName, StringComparison.OrdinalIgnoreCase) && m.CheckMethod(httpMethod));
				if(method.GetCustomAttribute<AuthenticationTypeAttribute>() == null)
				{
					var classAttr = Controller.GetCustomAttribute<AuthenticationTypeAttribute>();
					if (classAttr == null)
						return res;
					else
						return classAttr.AuthentcationType;
				}
				else
				{
					return method.GetCustomAttribute<AuthenticationTypeAttribute>().AuthentcationType;
				}
			}
			catch {
				throw new Exception($"Action method is not single in controller!{GetControllerName()} - {GetActionName()}");
			}
		}
	}
}
