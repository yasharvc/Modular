﻿using Contracts.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Contracts.Hub
{
	public class InvocationHub
	{
		private const string CallFunctionPath = "/Debug/CallFunction";
		private const string GetConnectionStringPath = "/Debug/ConnectionString";
		private const string GetModulesPath = "/Debug/GetModules";
		private const string GetUsersPath = "/Debug/GetUsers";
		private static string BaseUri = "http://localhost:2949/";

		public static IInvocationHubProvider InvocationHubProvider = null;
		public static bool IsModuleInDebugMode(bool? desired = null)
		{
			if (desired.HasValue)
				return desired.Value;
			return InvocationHubProvider == null;
		}

		public static string GetConnectionString()
		{
			if (IsModuleInDebugMode())
				return HttpPost(BaseUri, GetConnectionStringPath, new List<KeyValuePair<string, string>>());
			else
				return InvocationHubProvider.GetConnectionString();
		}

		private static string HttpPost(string BaseUrl, string FunctionPath, List<KeyValuePair<string, string>> parameters = null)
		{
			if (parameters == null)
				parameters = new List<KeyValuePair<string, string>>();
			var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
			var res = client.PostAsync(FunctionPath, new FormUrlEncodedContent(parameters)).Result;
			if (res.IsSuccessStatusCode)
			{
				string resultContent = res.Content.ReadAsStringAsync().Result;
				return (resultContent);
			}
			return "";
		}

		public static IEnumerable<ModuleManifest> GetModules()
		{
			if (IsModuleInDebugMode())
				return new List<ModuleManifest> {
					new DummyManifest("Dummy1","First dummy module","Description for first dummy module"),
					new DummyManifest("Dummy2","Second dummy module","Description for second dummy module"),
					new DummyManifest("Dummy3","Third dummy module","Description for third dummy module")
				};
			else
				return InvocationHubProvider.GetModuleList().Where(m => m.Status == ModuleStatus.Enable);
		}
	}
}