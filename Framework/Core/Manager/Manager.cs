using Manager.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager
{
	public class Manager
	{
		public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();
	}
}
