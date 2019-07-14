using ModulesFileUploader.FileConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModulesFileUploader.MVCFileUploader
{
	public class ViewsFileUploader : MVCFileUploader
	{
		List<string> ignorenceFiles = new List<string>
		{
			"_viewimports.cshtml",
			"_viewstart.cshtml"
		};
		public ViewsFileUploader(string moduleName) : base(moduleName)
		{
			SourcePathFolder = "Views";
			FileIgnorance = f => {
				if (ignorenceFiles.Contains(f.Name.ToLower()))
					return true;
				return false;
			};
			Convertors.Add(new CSHTMLFileConverter());
		}

		public ViewsFileUploader(string moduleName, string tempFolderName) : this(moduleName) => SubPath = tempFolderName;
	}
}