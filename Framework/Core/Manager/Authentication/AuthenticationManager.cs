using Contracts.Authentication;
using System.Collections.Generic;

namespace Manager.Authentication
{
	public class AuthenticationManager : IManager
	{
		private AnonymousAuthentication Anonymous { get; } = new AnonymousAuthentication();
		public ModuleAdministrationAuthentication ModuleAdministration { private get; set; } = null;
		protected Dictionary<string, AuthenticationResolver> Authentications { get; } = new Dictionary<string, AuthenticationResolver>();

		public void AddAuthentication(AuthenticationResolver authenticationResolver) => Authentications[authenticationResolver.Resolve().Token] = authenticationResolver;

		public string GenerateNewToken()
		{
			var generator = new AuthenticationGUIDMaker();
			var token = generator.GetNew();
			while (IsAuthenticationExists(token)) token = generator.GetNew();
			return token;
		}

		public bool IsAuthenticationExists(string token)
		{
			if (token == Anonymous.Token)
				return true;
			else if (IsModuleAdminToken(token))
				return true;
			return Authentications.ContainsKey(token);
		}

		public IEnumerable<Contracts.Authentication.Authentication> GetInstalledAuthentications()
		{
			List<Contracts.Authentication.Authentication> res = new List<Contracts.Authentication.Authentication>
			{
				Anonymous
			};
			if (IsAdminAuthenticationExists())
				res.Add(ModuleAdministration);
			foreach (var item in Authentications.Values)
				res.Add(item.Resolve());
			return res;
		}

		private bool IsAdminAuthenticationExists() => ModuleAdministration != null;

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
			if (token == Anonymous.Token)
				return Anonymous;
			else if (IsModuleAdminToken(token))
				return ModuleAdministration;
			else if (Authentications.ContainsKey(token))
				return Authentications[token].Resolve();
			throw new Contracts.Exceptions.System.AuthenticatonNotFoundException(token);
		}

		private bool IsModuleAdminToken(string token) => IsAdminAuthenticationExists() && token == ModuleAdministration.Token;
	}
}