using Contracts.Module;
using System.Reflection;

namespace Manager.Module
{
	public class ModuleResolver
	{
		public ModuleAssembly Assembly { get; private set; }
		public ModuleManifest GetModuleManifest() => Assembly.Manifest;

		public ModuleResolver(byte[] bytes)
		{
			Assembly = new ModuleAssembly(bytes);
		}

		public ModuleResolver(Assembly assembly) => Assembly = new ModuleAssembly(assembly);
	}
}
