using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RequestHandler.Tests
{
	[TestClass]
	public class RequestHandlerTest
	{
		[TestMethod]
		public void Empty_HeaderHost()
		{
			RequestHandler d = new RequestHandler(GetEmptyContext());
			d.GetRequestInformation();
			Assert.IsTrue(d.IsRequestInformationEmpty());
		}

		private HttpContext GetEmptyContext()
		{
			var res = new DefaultHttpContext();
			res.Request.Headers["HeaderHost"] = new StringValues();
			return res;
		}
		private HttpContext GetContextWithHeaders()
		{
			var res = new DefaultHttpContext();
			res.Request.Headers["HeaderHost"] = "Test";
			return res;
		}
	}
}
