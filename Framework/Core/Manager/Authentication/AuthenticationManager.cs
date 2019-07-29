using Contracts.Authentication;
using System.Collections.Generic;

namespace Manager.Authentication
{
	public class AuthenticationManager : IManager
	{
		protected Dictionary<string, IAuthenticationType> Authentications { get; } = new Dictionary<string, IAuthenticationType>();

		public void AddAuthentication(IAuthenticationType authenticationType) => Authentications[authenticationType.Token] = authenticationType;

		public string GenerateNewToken()
		{
			var generator = new AuthenticationGUIDMaker();
			var token = generator.GetNew();
			while (IsAuthenticationExists(token)) token = generator.GetNew();
			return token;
		}

		public bool IsAuthenticationExists(string token) => Authentications.ContainsKey(token);

		public IEnumerable<IAuthenticationType> GetInstalledAuthentications() => Authentications.Values;

		public bool Upload(byte[] content)
		{
			try
			{
				var fx = new AuthenticationResolver(content).Resolve();
				AddAuthentication(fx);
				return true;
			}
			catch
			{
				return false;
			}
		}


	}
}