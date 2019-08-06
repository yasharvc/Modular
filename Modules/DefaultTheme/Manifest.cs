using Contracts;
using Contracts.Module;
using Contracts.Module.Menu;
using Contracts.ViewComponent;
using System.Collections.Generic;
using System.Reflection;

namespace DefaultTheme
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest()
		{
			Name = "DefaultTheme";
			Description = "تم سازمانی شونیز";
			Token = "MDL-F09EEAA2-AC78-4F9A-A4DD-BAF52DD98885";
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}

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