using Manager.Authentication;
using Manager.Module;
using System;
using System.Collections.Generic;
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
		}
	}
}
