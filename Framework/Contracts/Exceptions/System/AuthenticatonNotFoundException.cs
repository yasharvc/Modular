using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Exceptions.System
{
	public class AuthenticatonNotFoundException : SystemException
	{
		public string MissedToken { get; set; }
		public AuthenticatonNotFoundException(string authenticationToken):base($"Authentication provider with token: {authenticationToken} not found!") { MissedToken = authenticationToken; }
	}
}