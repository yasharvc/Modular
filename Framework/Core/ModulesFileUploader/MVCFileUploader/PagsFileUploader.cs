using ModulesFileUploader.FileConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModulesFileUploader.MVCFileUploader
{
	public class PagesFileUploader : MVCFileUploader
	{
		public PagesFileUploader(string moduleName) : base(moduleName)
		{
			SourcePathFolder = "Pages";
			Convertors.Add(new CSHTMLFileConverter());
		}
		public PagesFileUploader(string moduleName, string tempFolderName) : this(moduleName) => SubPath = tempFolderName;
	}
}