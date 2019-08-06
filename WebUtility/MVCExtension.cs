using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace WebUtility
{
	public static class MVCExtension
	{
		public static HttpMethod GetMethod(this HttpRequest request)
		{
			var method = request.Method.ToLower().Trim();
			if (method.Equals("post"))
				return HttpMethod.Post;
			if (method.Equals("get"))
				return HttpMethod.Get;
			throw new Contracts.Exceptions.System.HttpMethodNotFoundException(request.Method);
		}
	}
}
