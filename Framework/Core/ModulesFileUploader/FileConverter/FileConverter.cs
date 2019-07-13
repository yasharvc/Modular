using System;
using System.Collections.Generic;
using System.Text;

namespace ModulesFileUploader.FileConverter
{
	public abstract class FileConverter
	{
		public string Convert(string fileName,string content)
		{
			if (IsFileNameAceptable(fileName))
				return ConvertContent(content);
			return content;
		}

		protected abstract string ConvertContent(string content);
		public abstract bool IsFileNameAceptable(string fileName);
	}
}
