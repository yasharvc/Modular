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
		public string CurrentThemeProvider => "";

		public string ThemeLayoutPath
		{
			get
			{
				if (string.IsNullOrEmpty(CurrentThemeProvider) || !ThemeProviders.ContainsKey(CurrentThemeProvider))
					return "_Layout";
				return $"~/{Consts.MODULES_BASE_PATH}/{CurrentThemeProvider}/{ThemeProviders[CurrentThemeProvider].LayoutPathInsideModule}";
			}
		}

		public void Upload(byte[] zipFile)
		{
			var tempFolder = new TempFolderMaker();
			tempFolder.CreateNewTempFolder();
			Unzipper unzipper = new Unzipper(zipFile, tempFolder.PathToTemp);
			var path = Directory.GetFiles(tempFolder.PathToTemp, "*.dll").First();
			ModuleResolver resolver = new ModuleResolver(File.ReadAllBytes(path));

			try
			{
				var theme = resolver.Assembly.GetThemeProvider();
				ThemeProviders[resolver.GetModuleManifest().Name] = theme;
			}
			catch { }
			new StaticFileUploader().Move(tempFolder.PathToTemp);
			new ViewsFileUploader(resolver.GetModuleManifest().Name).Move(tempFolder.PathToTemp);
			new PagesFileUploader(resolver.GetModuleManifest().Name).Move(tempFolder.PathToTemp);

			ModuleManager.AddModule(resolver.GetModuleManifest().Name, resolver.Assembly);
			RouterManager.ResolveRouteInformation(resolver.Assembly);

		}

		public ControllerExecuter.ControllerExecuter GetExecuter(string moduleName) => new ControllerExecuter.ControllerExecuter(ModuleManager.GetAssembly(moduleName));
	}
}
