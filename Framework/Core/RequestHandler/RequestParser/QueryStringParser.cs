using CoreCommons;
using System.Collections.Generic;

namespace RequestHandler.RequestParser
{
	public class QueryStringParser : IParser
	{
		public List<RequestParameter> Parse(string query)
		{
			if (query.StartsWith('?'))
				query = query.Substring(1);
			var res = new List<RequestParameter>();
			string[] parameters = query.Split('&');
			foreach (var parameter in parameters)
			{
				try
				{
					var extractedParameter = RequestHelper.ProcessKeyValue(parameter);
					res.Add(extractedParameter);
				}
				catch { }
			}
			return res;
		}
	}
}
