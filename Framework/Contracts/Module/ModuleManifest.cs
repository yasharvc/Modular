using Contracts.ViewComponent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Contracts.Module
{
	public abstract class ModuleManifest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Token { get; set; }
		public Version Version { get; set; }
		public List<Dependency> Dependencies { get; set; } = new List<Dependency>();
		public Dictionary<string, BaseViewComponent> HomePageViewComponents { get; } = new Dictionary<string, BaseViewComponent>();
		public Dictionary<string, BaseViewComponent> ViewComponents { get; } = new Dictionary<string, BaseViewComponent>();
		public abstract BaseViewComponent GetCustomViewComponent(string name);
	}
}