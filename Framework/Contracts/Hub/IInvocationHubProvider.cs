﻿using Contracts.Models;
using Contracts.Module;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Contracts.Hub
{
	public interface IInvocationHubProvider
	{
		string GetConnectionString();
		IEnumerable<ModuleManifest> GetModuleList();
		IEnumerable<User> GetUsers();
		User GetCurrentUser();
		object InvokeServiceFunction(string ModuleName, string FullClassName, string ServiceName, Type ReturnType, params dynamic[] Parameters);
	}
}