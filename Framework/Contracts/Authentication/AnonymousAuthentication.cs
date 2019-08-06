using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace Contracts.Authentication
{
	public class AnonymousAuthentication : Authentication
	{
		public override string LoginPagePath => throw new System.NotImplementedException();

		public override User GetCurrentUser(HttpContext ctx) => new User();

		public override string GetDescription() => "سطح دسترسی عمومی";

		public override bool IsAuthenticated() => true;
	}
}