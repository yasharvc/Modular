using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModulesFileUploader
{
	public class ModuleDllUploader
	{
		public string ModuleName { get; set; }

		public ModuleDllUploader(string moduleName) => ModuleName = moduleName;

		public void Move(string TempPath)
		{
			FileInfo info = new FileInfo(Directory.GetFiles(TempPath, $"*.dll").Single());
			File.Move(info.FullName, Path.Combine(Consts.MODULES_BASE_PATH, ModuleName, info.Name));
		}
	}
}
