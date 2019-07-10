using System;

namespace Contracts.Authentication
{
	public abstract class AuthenticationType : IAuthenticationType
	{
		public string Token { get; protected set; }
		public AuthenticationType() => Token = new AuthenticationGUIDMaker().GetAnonymouse();
		public AuthenticationType(Guid guid) => Token = new AuthenticationGUIDMaker().CreateCode(guid);

		public string GetCode()
		{
			var name = GetType().Name.Replace(".", "_");
			var template = $"using {nameof(Contracts)}.{nameof(Authentication)};\r\n\r\npublic sealed class {name} : AuthenticationType\r\n{{\r\n " +
				$"\tpublic {name}() : base() => Token = \"{Token}\";" +
				$"\r\n}}";
			return template;
		}

		public virtual string GetDescription() => "";
	}
}
