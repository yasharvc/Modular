using Contracts.Module.Menu;
using Contracts.ViewComponent;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contracts.Module
{
	public abstract class ModuleManifest
	{
		public ModuleManifest() { }
		public ModuleManifest(string name,string description, string token,params Dependency[] deps)
		{
			Name = name;
			Description = description;
			Token = token;
			Version = Assembly.GetExecutingAssembly().GetName().Version;
			Dependencies.AddRange(deps);
		}
		public string Name { get; set; }
		public string Description { get; set; }
		public string Token { get; set; }
		public Version Version { get; set; }
		public List<Dependency> Dependencies { get; set; } = new List<Dependency>();
		public abstract Dictionary<string, BaseViewComponent> HomePageViewComponents { get; }
		public abstract Dictionary<string, BaseViewComponent> ViewComponents { get; }
		public abstract BaseViewComponent GetCustomViewComponent(string name);
		public virtual IEnumerable<IMenu> Menus { get; } = new List<IMenu>();
		public virtual IEnumerable<IMenu> AdminMenu { get; } = new List<IMenu>();
		public ModuleStatus Status { get; set; } = ModuleStatus.Disable;
		public bool IsSystemModule { get; set; } = false;
		public virtual Dictionary<string, string> Redirections { get; } = new Dictionary<string, string>();

	}
}