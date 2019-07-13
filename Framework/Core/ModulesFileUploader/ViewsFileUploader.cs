using System;
using System.Collections.Generic;
using System.Text;

namespace ModulesFileUploader
{
	public class ViewsFileUploader : FolderedFileUploader
	{
		public ViewsFileUploader(string moduleName) : base(moduleName) { BasePath = "__"; }
	}
}