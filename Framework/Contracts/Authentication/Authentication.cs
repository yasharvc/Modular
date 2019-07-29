using Microsoft.AspNetCore.Http;

namespace Contracts.Authentication
{
	public abstract class Authentication : AuthenticationType, IAuthenticator
	{
		public abstract string LoginPagePath { get; }

		public abstract bool IsAuthenticated();

		public HttpContext HttpContext { get; set; }
	}
}