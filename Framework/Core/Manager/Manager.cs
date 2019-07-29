using Contracts;
using Contracts.Module;
using Manager.Authentication;
using Manager.Module;
using Manager.Router;
using ModulesFileUploader;
using ModulesFileUploader.MVCFileUploader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TempFolderManager;

namespace Manager
{
	public class Manager
	{

		public Manager() => LoadModules();

		private readonly List<string> SpecialFolders = new List<string> { "temp" };
		public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();
		public ModuleManager ModuleManager { get; } = new ModuleManager();
		public RouterManager RouterManager { get; } = new RouterManager();

		public IEnumerable<ModuleAssembly> Modules => ModuleManager.GetModules();

		public Dictionary<string, IThemeProvider> ThemeProviders { get; } = new Dictionary<string, IThemeProvider>();
		public string CurrentThemeProvider { get; private set; } = "";

		private List<ModuleIndexData> DependencyIndex { get; set; } = new List<ModuleIndexData>();

		public string ThemeLayoutPath
		{
			get
			{
				if (string.IsNullOrEmpty(CurrentThemeProvider) || !ThemeProviders.ContainsKey(CurrentThemeProvider))
					return "_Layout";
				return $"~/{Consts.MODULES_BASE_PATH}/{CurrentThemeProvider}{(ThemeProviders[CurrentThemeProvider].LayoutPathInsideModule.StartsWith("/") ? "" : "/")}{ThemeProviders[CurrentThemeProvider].LayoutPathInsideModule}";
			}
		}

		public void Upload(byte[] zipFile)
		{
			var tempFolder = new TempFolderMaker();
			tempFolder.CreateNewTempFolder();

			var unzipper = new Unzipper(zipFile, tempFolder.PathToTemp);
			var path = Directory.GetFiles(tempFolder.PathToTemp, "*.dll").First();
			var dllBytes = File.ReadAllBytes(path);

			var resolver = AddModule(dllBytes);

			ExtractFiles(tempFolder.PathToTemp, resolver.GetModuleManifest().Name);

			tempFolder.Delete();
		}

		private void GetTheme(ModuleResolver resolver)
		{
			string ModuleName = resolver.GetModuleManifest().Name;
			try
			{
				var theme = resolver.Assembly.GetThemeProvider();
				ThemeProviders[ModuleName] = theme;
				CurrentThemeProvider = ModuleName;
			}
			catch { }
		}

		private static void ExtractFiles(string tempFolder, string ModuleName)
		{
			new StaticFileUploader().Move(tempFolder);
			new ViewsFileUploader(ModuleName).Move(tempFolder);
			new PagesFileUploader(ModuleName).Move(tempFolder);
			new ModuleDllUploader(ModuleName).Move(tempFolder);
		}

		private ModuleResolver AddModule(byte[] dllBytes, bool ignoreDependencyIndexUpdate = true)
		{
			var resolver = new ModuleResolver(dllBytes);
			var manifest = resolver.GetModuleManifest();
			if (ignoreDependencyIndexUpdate)
				UpdateDependencyIndex(manifest);
			GetTheme(resolver);
			ModuleManager.AddModule(manifest.Name, resolver.Assembly);
			RouterManager.ResolveRouteInformation(resolver.Assembly);
			AuthenticationManager.Upload(dllBytes);
			return resolver;
		}

		private void UpdateDependencyIndex(ModuleManifest module)
		{
			var dependedModules = Modules.Where(m => m.Manifest.Dependencies.Any(k => k.ModuleName == module.Name));
			if (DependencyIndex.Any(m => m.ModuleName == module.Name))
			{
				DependencyIndex.Single(m => m.ModuleName == module.Name).Dep = module.Dependencies.Select(dp => dp.ModuleName).ToList();
				//Modules.Single(m => m.Value.Manifest.ModuleName == module.Manifest.ModuleName).Value.Manifest.Dependencies = module.Manifest.Dependencies;
				try
				{
					var oldModule = Modules.Single(m => m.Manifest.Name == module.Name);
					var dependOnModules = Modules.Where(m => oldModule.Manifest.Dependencies.Select(d => d.ModuleName).Contains(m.Manifest.Name));
					var targets = DependencyIndex.Where(dp => dependOnModules.Any(m => m.Manifest.Name == dp.ModuleName)).ToList();
					foreach (var target in targets)
						DependencyIndex.Single(d => d.ModuleName == target.ModuleName).Cnt--;
				}
				catch { }
			}
			else
			{
				var temp = new ModuleIndexData { Dep = module.Dependencies.Select(m => m.ModuleName).ToList(), Cnt = 0, ModuleName = module.Name };
				DependencyIndex.Add(temp);
			}
			foreach (var dependency in module.Dependencies)
				DependencyIndex.Single(m => m.ModuleName == dependency.ModuleName).Cnt++;
			File.WriteAllText(GetDiFilePath(), JsonConvert.SerializeObject(DependencyIndex));
		}
		private string GetDiFilePath() => Path.Combine(Consts.MODULES_BASE_PATH, "di.txt");
		public ControllerExecuter.ControllerExecuter GetExecuter(string moduleName) => new ControllerExecuter.ControllerExecuter(ModuleManager.GetAssembly(moduleName));

		public void LoadModules()
		{
			if (Directory.Exists(Consts.MODULES_BASE_PATH))
			{
				if (File.Exists(GetDiFilePath()))
				{
					var dependencyIndexJson = File.ReadAllText(GetDiFilePath());
					DependencyIndex = JsonConvert.DeserializeObject<List<ModuleIndexData>>(dependencyIndexJson);
					LoadModulesWithDependencyIndex();
				}
				else
				{
					LoadModulesWithFolders();
				}
			}
		}
		private void LoadModulesWithDependencyIndex()
		{
			DependencyIndex.Sort(
				delegate (ModuleIndexData d1, ModuleIndexData d2)
				{
					if (d1.Delta > d2.Delta) return -1;
					else if (d1.Delta < d2.Delta) return 1;
					else
					{
						if (d1.Cnt > d2.Cnt) return -1;
						else if (d1.Cnt < d2.Cnt) return 1;
						else return 0;
					}
				});
			foreach (var dp in DependencyIndex)
			{
				var folder = Directory.GetDirectories($"{Consts.MODULES_BASE_PATH}/").FirstOrDefault(m => m.ToLower().EndsWith(dp.ModuleName.ToLower()));
				AddModule(File.ReadAllBytes(Directory.GetFiles(folder, "*.dll").Single()), false);
			}
		}
		private void LoadModulesWithFolders()
		{
			var folders = Directory.GetDirectories($"{Consts.MODULES_BASE_PATH}/").Where(m => !SpecialFolders.Contains(m.Replace($"{Consts.MODULES_BASE_PATH}/", "").ToLower()));
			foreach (var folder in folders)
			{
				AddModule(File.ReadAllBytes(Directory.GetFiles(folder, "*.dll").Single()));
			}
		}
	}
}