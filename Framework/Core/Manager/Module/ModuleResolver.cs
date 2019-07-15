using Contracts.Module;

namespace Manager.Module
{
	public class ModuleResolver
	{
		ModuleAssembly Assembly { get; set; }
		public ModuleManifest GetModuleManifest() => Assembly.Manifest;

		public ModuleResolver(byte[] bytes)
		{
			Assembly = new ModuleAssembly(bytes);
		}
	}
}
