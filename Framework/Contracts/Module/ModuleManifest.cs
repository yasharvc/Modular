﻿using System;
using System.Collections.Generic;

namespace Contracts.Module
{
	public class ModuleManifest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Token { get; set; }
		public Version Version { get; set; }
		public List<Dependency> Dependencies { get; set; } = new List<Dependency>();
	}
}