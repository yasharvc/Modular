using Contracts.Authentication;
using System;
using System.Collections.Generic;

namespace Manager.Authentication
{
	public class AuthenticationManager : IManager
	{
		protected Dictionary<string, AuthenticationResolver> Authentications { get; } = new Dictionary<string, AuthenticationResolver>();

		public void AddAuthentication(AuthenticationResolver authenticationResolver) => Authentications[authenticationResolver.Resolve().Token] = authenticationResolver;

		public string GenerateNewToken()
		{
			var generator = new AuthenticationGUIDMaker();
			var token = generator.GetNew();
			while (IsAuthenticationExists(token)) token = generator.GetNew();
			return token;
		}

		public bool IsAuthenticationExists(string token) => Authentications.ContainsKey(token);

		public IEnumerable<Contracts.Authentication.Authentication> GetInstalledAuthentications()
		{
			List<Contracts.Authentication.Authentication> res = new List<Contracts.Authentication.Authentication>();
			foreach (var item in Authentications.Values)
			{
				res.Add(item.Resolve());
			}
			return res;
		}

		public bool Upload(byte[] content)
		{
			try
			{
				var AuthResolver = new AuthenticationResolver(content);
				var fx = AuthResolver.Resolve();
				AddAuthentication(AuthResolver);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public Contracts.Authentication.Authentication GetAuthenticationByToken(string token)
		{
			if(Authentications.ContainsKey(token))
				Authentications[token].Resolve();
			throw new Contracts.Exceptions.System.AuthenticatonNotFoundException(token);
		}

	}
}