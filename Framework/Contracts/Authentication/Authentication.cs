using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Authentication
{
	public abstract class Authentication : AuthenticationType, IAuthenticator
	{
		public abstract string LoginPagePath { get; }

		public abstract bool IsAuthenticated();
	}
}
