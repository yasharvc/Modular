using Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSThemeWithAuthentication
{
	public class BTAuthentication : Authentication
	{
		public BTAuthentication() => Token = "AUTH-E181FC39-2628-4335-BE94-EDFD60A680FD";

		public override string LoginPagePath => "/Security/Login";

		public override string GetDescription() => "دسترسی به سایت اصلی";

		public override bool IsAuthenticated()
		{
			throw new NotImplementedException();
		}
	}
}
