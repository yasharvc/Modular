using Contracts.Authentication;
using Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace TestWebApplication
{
	public class TestAuth : Authentication
	{

		public TestAuth(HttpContext context) => HttpContext = context;

		public override string LoginPagePath => "/Security/Login";

		public override bool IsAuthenticated() => HttpContext.Request.Cookies.ContainsKey(Token);

		public TestAuth()
		{
			Token = "AUTH-BA3F441A-4685-4139-A426-4E075F27440F";
		}

		internal bool Authenticate(string user,string pass)
		{
			if (user.Equals("yashar", StringComparison.OrdinalIgnoreCase) && pass.Equals("123", StringComparison.OrdinalIgnoreCase))
			{
				HttpContext.Response.Cookies.Append(Token, $"{user} - {pass}");
				return true;
			}
			return false;
		}

		public override User GetCurrentUser(HttpContext ctx)
		{
			throw new NotImplementedException();
		}
	}
}
