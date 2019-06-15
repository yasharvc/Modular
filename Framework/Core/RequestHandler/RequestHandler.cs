using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace RequestHandler
{
	public class RequestHandler
	{
		public RequestInformation RequestInformation { get; } = new RequestInformation();
		HttpContext Context { get; set; }
		public RequestHandler(HttpContext context)
		{
			Context = context;
		}

		public void GetRequestInformation()
		{
			var requestHeaders = Context.Request.Headers;//(HttpRequestHeaders)((DefaultHttpRequest)Context.Request).Headers;
			RequestInformation.HostURL = requestHeaders.ContainsKey("HeaderHost") && requestHeaders["HeaderHost"].Count > 0 ? requestHeaders["HeaderHost"][0] : "";
			RequestInformation.HeaderReferer = requestHeaders.ContainsKey("HeaderReferer") && requestHeaders["HeaderReferer"].Count > 0 ? requestHeaders["HeaderReferer"][0] : "";
			RequestInformation.UrlRequestPart = RequestInformation.HeaderReferer.Substring(RequestInformation.HeaderReferer.IndexOf(RequestInformation.HostURL) + RequestInformation.HostURL.Length);
		}

		public bool IsRequestInformationEmpty() => RequestInformation.HostURL.Length == 0 && RequestInformation.HeaderReferer.Length == 0 && RequestInformation.UrlRequestPart.Length == 0;
	}
}
