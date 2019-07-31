using Contracts.Module.Menu;
using Contracts.ViewComponent;
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
		public abstract Dictionary<string, BaseViewComponent> HomePageViewComponents { get; }
		public abstract Dictionary<string, BaseViewComponent> ViewComponents { get; }
		public abstract BaseViewComponent GetCustomViewComponent(string name);
		public virtual IEnumerable<IMenu> Menus { get; } = new List<IMenu>();
		public ModuleStatus Status { get; set; } = ModuleStatus.Disable;
	}
}