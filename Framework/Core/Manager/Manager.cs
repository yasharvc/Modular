using Manager.Authentication;
using Manager.Module;
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

		public void Upload(byte[] zipFile)
		{
			var tempFolder = new TempFolderMaker();
			tempFolder.CreateNewTempFolder();
			Unzipper unzipper = new Unzipper(zipFile, tempFolder.PathToTemp);
			var path = Directory.GetFiles(tempFolder.PathToTemp, "*.dll").First();
			ModuleResolver resolver = new ModuleResolver(File.ReadAllBytes(path));
			new StaticFileUploader().Move(tempFolder.PathToTemp);
			new ViewsFileUploader(resolver.GetModuleManifest().Name).Move(tempFolder.PathToTemp);
			new PagesFileUploader(resolver.GetModuleManifest().Name).Move(tempFolder.PathToTemp);
		}
	}
}
