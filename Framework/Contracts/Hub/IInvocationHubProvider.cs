using Contracts.Models;
using Contracts.Module;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static Contracts.Delegates;

namespace Contracts.Hub
{
	public interface IInvocationHubProvider
	{
		event UserInfoEventArg OnCurrentUserRequired;
		string GetConnectionString();
		IEnumerable<ModuleManifest> GetModuleList();
		User GetCurrentUser(HttpContext ctx);
	}
}