using Contracts;
using Contracts.Module;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Module
{
	public class ModuleManager : IManager
	{
		protected Dictionary<string, Assembly> ModuleAssemblies { get; } = new Dictionary<string, Assembly>();
		public string GenerateNewToken() => new ModuleGUIDMaker().GetNew();

		public void AddModule(string ModuleName, Assembly assembly) => ModuleAssemblies[ModuleName] = assembly;

		public Assembly GetAssembly(string ModuleName)
		{
			if (!ModuleAssemblies.ContainsKey(ModuleName))
				LoadModuleDll(ModuleName);
			return ModuleAssemblies[ModuleName];
		}

		private void LoadModuleDll(string ModuleName)
		{
			var path = GetModuleDirectory(ModuleName);
			var file = Directory.GetFiles(path, "*.dll").Single();
			ModuleAssemblies[ModuleName] = Assembly.Load(File.ReadAllBytes(file));
		}

		private static string GetModuleDirectory(string ModuleName) => Path.Combine(Consts.MODULES_BASE_PATH, ModuleName);

		public void ReloadModule(string ModuleName) => LoadModuleDll(ModuleName);

		public void RemoveModule(string ModuleName)
		{
			if (ModuleAssemblies.ContainsKey(ModuleName))
				ModuleAssemblies.Remove(ModuleName);
		}
	}
}
