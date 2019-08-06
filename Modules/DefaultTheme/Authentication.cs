using Contracts.Authentication;

namespace DefaultTheme
{
	public class Authentication : Contracts.Authentication.Authentication
	{
		public Authentication() => Token = "AUTH-4BAF4B6F-8DB9-4955-8EF7-B039875EB169";

		public override string LoginPagePath => "/Security/Login";

		public override string GetDescription() => "دسترسی به سایت اصلی";

		public override bool IsAuthenticated() => new UserAuthenticator().IsAuthenticated(HttpContext);
	}
}