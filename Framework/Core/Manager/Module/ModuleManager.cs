using Contracts.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Module
{
	public class ModuleManager : IManager
	{
		protected Dictionary<string,Ass>
		public string GenerateNewToken() => new ModuleGUIDMaker().GetNew();
	}
}
