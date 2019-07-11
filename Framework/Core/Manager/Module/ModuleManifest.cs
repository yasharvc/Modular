using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Module
{
	public class ModuleManifest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Token { get; set; }
		public Version Version { get; set; }
	}
}
