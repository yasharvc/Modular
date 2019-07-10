using System;

namespace Contracts.Authentication
{
	public class AuthenticationGUIDMaker
	{
		public string GetNew() => CreateCode(Guid.NewGuid());

		internal string GetAnonymouse() => CreateCode(new Guid());

		internal string CreateCode(Guid guid) => $"AUTH-{guid.ToString().ToUpper()}";
	}
}
