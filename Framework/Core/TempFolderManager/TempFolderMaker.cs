using System;
using System.IO;

namespace TempFolderManager
{
	public class TempFolderMaker
	{
		public const string ModuleBasePath = "__";
		public const string TempFolderBasePath = ModuleBasePath + "\\Temp";
		public string PathToTemp { get; protected set; }
		public string CreateNewTempFolder()
		{
			var newGUID = AcequireGUID();
			var path = Path.Combine(TempFolderBasePath, newGUID);
			Directory.CreateDirectory(path);
			PathToTemp = path;
			return PathToTemp;
		}

		private string AcequireGUID() => Guid.NewGuid().ToString();
	}
}