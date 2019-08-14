using Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Modular.Classes
{
	public class AdminAuthentication : Contracts.Authentication.ModuleAdministrationAuthentication
	{
		public override string LoginPagePath => "/_ModulesAdministration/Security/Login";

		public override User GetCurrentUser(HttpContext ctx)
		{
			throw new NotImplementedException();
		}

		public override bool IsAuthenticated()
		{
			throw new NotImplementedException();
		}
	}
}
