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

		internal void Extend()
		{
			throw new NotImplementedException();
		}

		internal bool Authenticate(string userName, string password)
		{
			var auth = new Authentication();
			var token = auth.Authenticate(userName, password, UserType.SiteManager);
			if (!string.IsNullOrEmpty(token))
			{
				Authenticate(ctrl, token);
				return true;
			}
			return false;
		}

		internal void Disprove()
		{
			throw new NotImplementedException();
		}
	}
}
