using Contracts.Authentication;
using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace BSThemeWithAuthentication
{
	public class BTAuthentication : Authentication
	{
		public BTAuthentication() => Token = "AUTH-E181FC39-2628-4335-BE94-EDFD60A680FD";

		public override string LoginPagePath => "/Security/Login";

		public override User GetCurrentUser(HttpContext ctx)
		{
			throw new System.NotImplementedException();
		}

		public override string GetDescription() => "دسترسی به سایت اصلی";

		public override bool IsAuthenticated() => (HttpContext.Request.Cookies.ContainsKey("FX"));
	}
}