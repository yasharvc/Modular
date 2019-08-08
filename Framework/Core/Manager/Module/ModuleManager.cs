using Contracts;
using Contracts.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Module
{
	public class ModuleManager : IManager
	{
		protected Dictionary<string, Tuple<ModuleAssembly, ModuleMeta>> ModuleAssemblies { get; } = new Dictionary<string, Tuple<ModuleAssembly, ModuleMeta>>();
		
		public string GenerateNewToken() => new ModuleGUIDMaker().GetNew();

		public void AddModule(ModuleManifest manifest, Assembly assembly) => ModuleAssemblies[manifest.Name] = new Tuple<ModuleAssembly, ModuleMeta>(new ModuleAssembly(assembly), new ModuleMeta(assembly, manifest));

		public Assembly GetAssembly(string ModuleName)
		{
			if (!ModuleAssemblies.ContainsKey(ModuleName))
				LoadModuleDll(ModuleName);
			return ModuleAssemblies[ModuleName].Item1;
		}

		private void LoadModuleDll(string ModuleName)
		{
			var path = GetModuleDirectory(ModuleName);
			var file = Directory.GetFiles(path, "*.dll").Single();
			var asm = new ModuleAssembly(Assembly.Load(File.ReadAllBytes(file)));
			ModuleAssemblies[ModuleName] = new Tuple<ModuleAssembly, ModuleMeta>(asm, new ModuleMeta(asm, asm.Manifest));
		}

		private static string GetModuleDirectory(string ModuleName) => Path.Combine(Consts.MODULES_BASE_PATH, ModuleName);

		public void ReloadModule(string ModuleName) => LoadModuleDll(ModuleName);

		public void RemoveModule(string ModuleName)
		{
			if (ModuleAssemblies.ContainsKey(ModuleName))
				ModuleAssemblies.Remove(ModuleName);
		}

		public IEnumerable<ModuleAssembly> GetModules() => ModuleAssemblies.Values.Select(m => m.Item1);

		public ModuleAssembly this[string name] => ModuleAssemblies[name].Item1;

		public void ChangeModuleStatus(string moduleName, ModuleStatus status) => ModuleAssemblies[moduleName].Item1.ChangeStatus(status);

		public string GetModuleCode(string moduleName) => ModuleAssemblies[moduleName].Item2.ServiceMeta.ToString();

		public ModuleMeta GetModuleMeta(string moduleName) => ModuleAssemblies[moduleName].Item2;

		public object CallModuleFunction(string moduleName,string fullClassName,string functionName,params object[] parameters)
		{
			var moduleMeta = GetModuleMeta(moduleName);
			var obj = moduleMeta.CreateObject(fullClassName);
			return moduleMeta.InvokeMethod(obj, functionName, parameters);
		}
	}
}