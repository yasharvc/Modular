using Manager.Authentication;
using Manager.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager
{
	public class Manager
	{
		public AuthenticationManager AuthenticationManager { get; } = new AuthenticationManager();
		public ModuleManager ModuleManager { get; } = new ModuleManager();
	}
}
