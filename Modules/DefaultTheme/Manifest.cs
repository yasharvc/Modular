using Contracts;
using Contracts.Module;
using Contracts.Module.Menu;
using Contracts.ViewComponent;
using System.Collections.Generic;

namespace DefaultTheme
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest() : base(nameof(DefaultTheme), "تم سازمانی شونیز", "MDL-F09EEAA2-AC78-4F9A-A4DD-BAF52DD98885")
		{ }

		public string LayoutPathInsideModule => "/Views/Shared/_Layout.cshtml";

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents
		{
			get
			{
				var components = new Dictionary<string, BaseViewComponent>();
				return components;
			}
		}

		public override Dictionary<string, BaseViewComponent> ViewComponents => new Dictionary<string, BaseViewComponent>();

		public override BaseViewComponent GetCustomViewComponent(string name)
		{
			if(name.Equals("ModuleMenu",System.StringComparison.OrdinalIgnoreCase))
				return new Components.ModulesMenuViewComponent();
			return null;
		}

		public override IEnumerable<IMenu> Menus
		{
			get
			{
				List<IMenu> res = new List<IMenu>();
				return res;
			}
		}
	}
}