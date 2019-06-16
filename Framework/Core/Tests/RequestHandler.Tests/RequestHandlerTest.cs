using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Primitives;

namespace RequestHandler.Tests
{
	[TestClass]
	public class RequestHandlerTest
	{
		[TestMethod]
		public void Empty_HttpContext()
		{
			RequestHandler d = new RequestHandler(GetEmptyContext());
			d.GetRequestInformation();
			Assert.IsTrue(d.IsRequestInformationEmpty());
		}

		[TestMethod]
		public void Just_HeaderHost()
		{
			RequestHandler d = new RequestHandler(GetContextWithHeaders("localhost:2929"));
			d.GetRequestInformation();

			var expected = "localhost:2929";

			Assert.AreEqual(expected, d.RequestInformation.HeaderHost);
			Assert.AreEqual("", d.RequestInformation.HeaderReferer);
			Assert.AreEqual("", d.RequestInformation.UrlRequestPart);
		}

		[TestMethod]
		public void HeaderHost_HeaderReferer()
		{
			RequestHandler d = new RequestHandler(GetContextWithHeaders("localhost:2929", "localhost:2929/a/b"));
			d.GetRequestInformation();

			var expected = "localhost:2929";

			Assert.AreEqual(expected, d.RequestInformation.HeaderHost);
			Assert.AreEqual("localhost:2929/a/b", d.RequestInformation.HeaderReferer);
			Assert.AreEqual("/a/b", d.RequestInformation.UrlRequestPart);
		}

		private HttpContext GetEmptyContext()
		{
			var res = new DefaultHttpContext();
			return res;
		}
		private HttpContext GetContextWithHeaders(string HeaderHost = "Test", string HeaderReferer = "")
		{
			var res = GetEmptyContext();
			var req = res.Request;
			var keys = new HttpRequestHeaders();
			req.Headers.Append("HeaderHost", new StringValues(HeaderHost));
			req.Headers.Append("HeaderReferer", new StringValues(HeaderReferer));
			return res;
		}
	}
}
