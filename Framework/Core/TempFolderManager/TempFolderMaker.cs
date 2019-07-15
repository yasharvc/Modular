using System;
using System.IO;

namespace TempFolderManager
{
	public class TempFolderMaker
	{
		public const string ModuleBasePath = "__";
		public const string TempFolderBasePath = ModuleBasePath + "\\Temp";
		public string PathToTemp { get; protected set; }
		public string TempGuid { get; private set; } = "";
		public string CreateNewTempFolder()
		{
			TempGuid = AcequireGUID();
			var path = Path.Combine(TempFolderBasePath, TempGuid);
			Directory.CreateDirectory(path);
			PathToTemp = path;
			return PathToTemp;
		}

		private string AcequireGUID() => Guid.NewGuid().ToString();
	}
}