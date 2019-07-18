using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModulesFileUploader.MVCFileUploader
{
	public class MVCFileUploader : FolderedFileUploader
	{
		public MVCFileUploader(string folder) : base(folder) => BasePath = Consts.MODULES_BASE_PATH;
	}
}
