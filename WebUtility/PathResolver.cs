using ModuloContracts.Exceptions.SystemExceptions;
using ModuloContracts.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebUtility
{
	public class PathResolver
	{
		public PathParts GetPathParts(string path)
		{
			var hasQueryString = path.Contains('?');
			var queryString = hasQueryString ? path.Substring(path.IndexOf('?') + 1) : "";
			return GetPathParts(hasQueryString ? path.Substring(0, path.IndexOf('?')) : path, queryString);
		}
		public PathParts GetPathParts(string path,string queryString)
		{
			if (queryString.StartsWith("?"))
				queryString = queryString.Substring(1);
			var paths = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
			if (paths.Count() > 3)
			{
				return new PathParts
				{
					Area = paths[0],
					ModuleName = paths[1],
					Controller = paths[2],
					Action = paths[3].Split('?')[0],
					QueryString = queryString
				};
			}
			else if (paths.Count() == 3)
			{
				return new PathParts
				{
					Area = "",
					ModuleName = paths[0],
					Controller = paths[1],
					Action = paths[2].Split('?')[0],
					QueryString = queryString
				};
			}
			else if (paths.Count() == 2)
			{
				return new PathParts
				{
					Area = "",
					ModuleName = "",
					Controller = paths[0],
					Action = paths[1].Split('?')[0],
					QueryString = queryString
				};
			}
			else if (paths.Count() == 0)
			{
				var res = new PathParts();
				res.QueryString = queryString;
				return res;
			}
			throw new UnknownUrlException($"{path}{(queryString.Length > 0 ? "?" : "")}{queryString}");
		}
	}
}