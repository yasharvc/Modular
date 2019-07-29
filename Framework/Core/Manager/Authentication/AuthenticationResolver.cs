using Manager.Exceptions;
using System;
using System.Linq;
using System.Reflection;

namespace Manager.Authentication
{
	public class AuthenticationResolver
	{
		private Assembly Assembly { get; set; }

		public AuthenticationResolver(byte[] assemblyBytes)
		{
			Assembly = Assembly.Load(assemblyBytes);
		}

		public Contracts.Authentication.Authentication Resolve()
		{
			if (Assembly == null)
				throw new AssemblyIsNullException();
			var authType = Assembly.GetTypes().Single(m => m.IsSubclassOf(typeof(Contracts.Authentication.Authentication)));
			if (authType == null)
				throw new AuthenticationTypedClassNotFoundException();
			try
			{
				return Activator.CreateInstance(authType) as Contracts.Authentication.Authentication;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
