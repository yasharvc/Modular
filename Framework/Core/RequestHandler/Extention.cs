using Microsoft.AspNetCore.Http;
using System.IO;

namespace RequestHandler
{
	internal static class Extention
	{
		public static string BodyToString(this HttpRequest request)
		{
			if (request.Body != null && request.Body.CanRead)
			{
				using (var reader = new StreamReader(request.Body))
				{
					var body = reader.ReadToEnd();
					return body;
				}
			}
			return "";
		}
	}
}
