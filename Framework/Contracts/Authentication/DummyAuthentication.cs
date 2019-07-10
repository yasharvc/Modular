using System;

namespace Contracts.Authentication
{
	public class DummyAuthentication : AuthenticationType
	{
		public DummyAuthentication() : base(Guid.NewGuid()) { }
		public DummyAuthentication(Guid guid) : base(guid) { }

		public override string GetDescription() => "سطح دسترسی تستی";
	}
}