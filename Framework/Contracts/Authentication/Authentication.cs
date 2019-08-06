using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace Contracts.Authentication
{
	public abstract class Authentication : AuthenticationType, IAuthenticator
	{
		public abstract string LoginPagePath { get; }

		public abstract bool IsAuthenticated();

		public abstract User GetCurrentUser(HttpContext ctx);

		public HttpContext HttpContext { get; set; }
	}
}