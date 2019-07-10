using System;

namespace Contracts.Authentication
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class AuthenticationTypeAttribute : Attribute
	{
		public IAuthenticationType AuthentcationType { get; private set; }
		public AuthenticationTypeAttribute(Type authentcationType)
		{
			AuthentcationType = Activator.CreateInstance(authentcationType) as IAuthenticationType;
			if (AuthentcationType == null)
				throw new Exception($"Type is not IAuthentcationType type [{authentcationType.FullName}]");
		}
	}
}
