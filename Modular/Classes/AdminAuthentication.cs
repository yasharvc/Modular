using Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Modular.Classes
{
	public class AdminAuthentication : Contracts.Authentication.ModuleAdministrationAuthentication
	{
		AdminUserAuthentication auth = new AdminUserAuthentication();
		public override string LoginPagePath => "/_ModulesAdministration/Security/Login";

		public override User GetCurrentUser(HttpContext ctx) => auth.GetUser(HttpContext);

		public override bool IsAuthenticated() => auth.IsAuthenticated(HttpContext);

		internal bool Extend() => auth.ExtendTokenTime(HttpContext);

		internal bool Authenticate(string userName, string password) => auth.Authenticate(HttpContext, userName, password);

		internal void Disprove() => auth.Disprove(HttpContext);
	}
}
