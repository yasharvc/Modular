using Contracts.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TestWebApplication
{
	public class Manifest : ModuleManifest
	{
		public Manifest()
		{
			Name = "TestModule";
			Description = "ماژول تستی";
			Token = "MDL-7F3B064B-C033-4D5C-83C4-EDDED38B5A18";
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}
	}
}
