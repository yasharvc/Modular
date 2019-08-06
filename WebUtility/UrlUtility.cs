using System;
using System.Collections.Generic;
using System.Text;

namespace WebUtility
{
	public class UrlUtility
	{
		public bool IsStaticFile(string path)
		{
			var first = path.Split('?')[0];
			var lastpart = first.Substring(first.LastIndexOf('/'));
			return lastpart.Contains(".");
		}
	}
}
