using Contracts.Module;
using System.Collections.Generic;

namespace Contracts.Hub
{
	public interface IInvocationHubProvider
	{
		string GetConnectionString();
		IEnumerable<ModuleManifest> GetModuleList();
	}
}