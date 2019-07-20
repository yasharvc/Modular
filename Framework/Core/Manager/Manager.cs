using Contracts;
using Manager.Authentication;
using Manager.Module;
using Manager.Router;
using ModulesFileUploader;
using ModulesFileUploader.MVCFileUploader;
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
		public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();
		public ModuleManager ModuleManager { get; } = new ModuleManager();
		public RouterManager RouterManager { get; } = new RouterManager();

		public Dictionary<string, IThemeProvider> ThemeProviders { get; } = new Dictionary<string, IThemeProvider>();
		public string CurrentThemeProvider { get; private set; } = "";

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
			Unzipper unzipper = new Unzipper(zipFile, tempFolder.PathToTemp);
			var path = Directory.GetFiles(tempFolder.PathToTemp, "*.dll").First();
			var dllBytes = File.ReadAllBytes(path);
			ModuleResolver resolver = new ModuleResolver(dllBytes);

			string ModuleName = resolver.GetModuleManifest().Name;
			try
			{
				var theme = resolver.Assembly.GetThemeProvider();
				ThemeProviders[ModuleName] = theme;
				CurrentThemeProvider = ModuleName;
			}
			catch { }


			new StaticFileUploader().Move(tempFolder.PathToTemp);

			new ViewsFileUploader(ModuleName).Move(tempFolder.PathToTemp);
			new PagesFileUploader(ModuleName).Move(tempFolder.PathToTemp);
			new ModuleDllUploader(ModuleName).Move(tempFolder.PathToTemp);

			ModuleManager.AddModule(ModuleName, resolver.Assembly);
			RouterManager.ResolveRouteInformation(resolver.Assembly);
			AuthenticationManager.Upload(dllBytes);

			tempFolder.Delete();
		}

		public ControllerExecuter.ControllerExecuter GetExecuter(string moduleName) => new ControllerExecuter.ControllerExecuter(ModuleManager.GetAssembly(moduleName));
	}
}
