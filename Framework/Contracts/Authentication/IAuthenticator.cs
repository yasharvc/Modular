using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace Contracts.Authentication
{
	public interface IAuthenticator
	{
		string LoginPagePath { get; }
		bool IsAuthenticated();
		User GetCurrentUser(HttpContext ctx);
	}
}