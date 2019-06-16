using CoreCommons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace RequestHandler
{
	public static class RequestHelper
	{
		public static RequestParameter ProcessKeyValue(string parameter, StringToStringFunction decoder = null)
		{
			if (decoder == null)
				decoder = UrlDecode;
			if (parameter.Contains('='))
			{
				var key = UrlDecode(HtmlDecode(parameter.Split('=')[0]));
				var property = "";
				var index = "";
				if (key.Contains('[') && key.Contains(']'))
				{
					if (!key.Contains("]["))
					{
						var start = key.IndexOf('[');
						var finish = key.IndexOf(']', start);
						property = key.Substring(start + 1, finish - start - 1);
						key = key.Substring(0, start);
					}
					else
					{
						var start = key.IndexOf('[');
						var finish = key.IndexOf(']', start);
						index = key.Substring(start + 1, finish - start - 1);
						start = key.IndexOf("][") + 2;
						finish = key.IndexOf(']', start);
						property = key.Substring(start, finish - start);
						key = key.Substring(0, key.IndexOf('['));
					}
				}
				var value = UrlDecode(HtmlDecode(parameter.Split('=')[1]));
				return new RequestParameter { PropertyName = property, Name = key, Value = value, Index = index };
			}
			else
			{
				throw new Exception("مقدار داده شده اشتباه است");
			}
		}

		public static string UrlDecode(string s) => HttpUtility.UrlDecode(s);
		public static string HtmlDecode(string s) => HttpUtility.HtmlDecode(s);
	}
}
