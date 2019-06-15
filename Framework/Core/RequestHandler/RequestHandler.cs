using Contracts;
using Microsoft.AspNetCore.Http;
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
			if (Context.Request.Headers is HeaderDictionary requestHeaders)
				GetHeaderDictionaryHeaders(requestHeaders);
			else
				GetHttpRequestHeaders();
		}

		private void GetHeaderDictionaryHeaders(HeaderDictionary requestHeaders)
		{
			RequestInformation.HeaderHost = requestHeaders.ContainsKey("HeaderHost") && requestHeaders["HeaderHost"].Count > 0 ? requestHeaders["HeaderHost"][0] : "";
			RequestInformation.HeaderReferer = requestHeaders.ContainsKey("HeaderReferer") && requestHeaders["HeaderReferer"].Count > 0 ? requestHeaders["HeaderReferer"][0] : "";
			var index = RequestInformation.HeaderReferer.IndexOf(RequestInformation.HeaderHost);
			RequestInformation.UrlRequestPart = index >= 0 ? RequestInformation.HeaderReferer.Substring(index + RequestInformation.HeaderHost.Length) : RequestInformation.HeaderReferer;
		}

		private void GetHttpRequestHeaders()
		{
			var requestHeaders = (HttpRequestHeaders)Context.Request.Headers;
			RequestInformation.HeaderHost = requestHeaders.HeaderHost;
			RequestInformation.HeaderReferer = requestHeaders.HeaderReferer;
			var index = RequestInformation.HeaderReferer.IndexOf(RequestInformation.HeaderHost);
			RequestInformation.UrlRequestPart = index >= 0 ? RequestInformation.HeaderReferer.Substring(index + RequestInformation.HeaderHost.Length) : RequestInformation.HeaderReferer;
		}

		public bool IsRequestInformationEmpty() => RequestInformation.HeaderHost.Length == 0 && RequestInformation.HeaderReferer.Length == 0 && RequestInformation.UrlRequestPart.Length == 0;
	}
}
