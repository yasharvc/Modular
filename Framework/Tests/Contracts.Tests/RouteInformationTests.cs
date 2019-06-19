using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contracts.Tests
{
	[TestClass]
	public class RouteInformationTests
	{
		[TestMethod]
		public void GetQueryString_Simple()
		{
			RouteInformation route = new RouteInformation
			{
				Path = "/a/b/"
			};

			var excpected = "";

			Assert.AreEqual(excpected, route.GetQueryString("/a/b/"));
		}

		[TestMethod]
		public void GetQueryString_Simple_WithoutEndingSlash()
		{
			RouteInformation route = new RouteInformation
			{
				Path = "/a/b/"
			};

			var excpected = "";

			Assert.AreEqual(excpected, route.GetQueryString("/a/b"));
		}
		[TestMethod]
		public void GetQueryString_WithSlashedParamerter()
		{
			var url = "/a/b/c";
			RouteInformation route = new RouteInformation
			{
				Path = url
			};

			var excpected = "/c";

			Assert.AreEqual(excpected, route.GetQueryString(url));
		}
		[TestMethod]
		public void GetQueryString_WithSlashedParamerterAndQuestionQuery()
		{
			var url = "/a/b/c?id=100";
			RouteInformation route = new RouteInformation
			{
				Path = url
			};

			var excpected = "/c?id=100";

			Assert.AreEqual(excpected, route.GetQueryString(url));
		}
	}
}
