using CoreCommons;
using System;
using System.Collections.Generic;

namespace RequestHandler
{
	public class RequestBodyParser
	{
		public List<RequestParameter> Process(string body)
		{
			if (body.EndsWith("\r\n"))
			{
				return ProcessWithEnter(body);
			}
			else if (body.Contains("&"))
			{
				return ProcessWithAmpercent(body);
			}
			else if (body.Contains("="))
			{
				return ProcessWithEqual(body);
			}
			return new List<RequestParameter>();
		}

		private List<RequestParameter> ProcessWithEqual(string body) => new List<RequestParameter> { RequestHelper.ProcessKeyValue(body, RequestHelper.HtmlDecode) };

		private List<RequestParameter> ProcessWithAmpercent(string body) => SplitByDelimeter(body);

		private static List<RequestParameter> SplitByDelimeter(string body, string delimeter = "&")
		{
			var res = new List<RequestParameter>();
			var parameters = body.Split(new string[] { delimeter }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var parameter in parameters)
			{
				res.Add(RequestHelper.ProcessKeyValue(parameter, RequestHelper.HtmlDecode));
			}
			return res;
		}

		private List<RequestParameter> ProcessWithEnter(string body) => SplitByDelimeter(body, "\r\n");
	}
}
