using CoreCommons;
using System.Collections.Generic;

namespace RequestHandler.RequestParser
{
	public interface IParser
	{
		List<RequestParameter> Parse(string query);
	}
}
