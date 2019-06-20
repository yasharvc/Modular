using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Primitives;

namespace RequestHandler.Tests
{
	[TestClass]
	public class RequestHandlerTests
	{
		[TestMethod]
		public void Empty_HttpContext()
		{
			RequestInformation d = new RequestInformation(GetEmptyContext());
			d.ParseRequestData();
			Assert.IsTrue(d.IsRequestInformationEmpty());
		}

		[TestMethod]
		public void Just_HeaderHost()
		{
			RequestInformation d = new RequestInformation(GetContextWithHeaders("localhost:2929"));
			d.ParseRequestData();

			var expected = "localhost:2929";

			Assert.AreEqual(expected, d.RequestURLInformation.HeaderHost);
			Assert.AreEqual("", d.RequestURLInformation.HeaderReferer);
			Assert.AreEqual("", d.RequestURLInformation.UrlRequestPart);
		}

		[TestMethod]
		public void HeaderHost_HeaderReferer()
		{
			RequestInformation d = new RequestInformation(GetContextWithHeaders("localhost:2929", "localhost:2929/a/b"));
			d.ParseRequestData();

			var expected = "localhost:2929";

			Assert.AreEqual(expected, d.RequestURLInformation.HeaderHost);
			Assert.AreEqual("localhost:2929/a/b", d.RequestURLInformation.HeaderReferer);
			Assert.AreEqual("/a/b", d.RequestURLInformation.UrlRequestPart);
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
