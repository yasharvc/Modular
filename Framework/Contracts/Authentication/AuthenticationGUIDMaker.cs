using Contracts.Contracts;
using System;

namespace Contracts.Authentication
{
	public class AuthenticationGUIDMaker : GUIDMaker
	{
		public AuthenticationGUIDMaker() : base("AUTH") { }
	}
}
