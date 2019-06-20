using Contracts.Enums;
using System;
using System.Collections.Generic;
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

		public string GetControllerName() => Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
		public string GetActionName() => Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];

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
			string path = $"/{GetControllerName()}/{GetActionName()}/";
			if (fixedURL.StartsWith(path, StringComparison.OrdinalIgnoreCase))
			{
				return url.Substring(path.Length - (!haveFirstSlash ? 1 : 0) - (!haveLastSlash ? 1 : 0));
			}
			else
			{
				return url;
			}
		}
	}
}
