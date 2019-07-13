using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModulesFileUploader.FileConverter
{
	public class CSHTMLFileConverter : FileConverter
	{
		public override bool IsFileNameAceptable(string fileName) =>
			fileName.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase);

		protected override string ConvertContent(string content)
		{
			var regEx = new Regex(@"((?<=\@model\s)[^\r|^\n]+)");
			var matches = regEx.Matches(content);
			content = regEx.Replace(content, "dynamic");
			regEx = new Regex(@"[(][\w.]+[)]");
			content = regEx.Replace(content, "(dynamic)");
			return content;
		}
	}
}
