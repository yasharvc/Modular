using BSThemeWithAuthentication.Components;
using BSThemeWithAuthentication.Controllers;
using Contracts;
using Contracts.Module;
using Contracts.Module.Menu;
using Contracts.ViewComponent;
using System.Collections.Generic;

namespace BSThemeWithAuthentication
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest() : base("BTThemeWithAuth", "تم ساده بوت استرپ همراه لایه امنیتی", "MDL-56DFF4F9-91DE-4B70-A1B7-2A78EAD96363") { }

		public string LayoutPathInsideModule => "/Views/Shared/_Layout.cshtml";

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents
		{
			get
			{
				var components = new Dictionary<string, BaseViewComponent>();
				components["KPI"] = new KPIViewComponent();
				return components;
			}
		}

		public override Dictionary<string, BaseViewComponent> ViewComponents => new Dictionary<string, BaseViewComponent>();

		public override BaseViewComponent GetCustomViewComponent(string name)
		{
			if (name.Equals("kpi", System.StringComparison.OrdinalIgnoreCase))
				return new KPIViewComponent();
			return null;
		}

		public override IEnumerable<IMenu> Menus
		{
			get
			{
				List<IMenu> res = new List<IMenu>();
				var menu = new Menu
				{
					Title = "تست",
					Icon = "fa fa-cogs",
					Link = new Link { Action = nameof(GridTestController.Index), Controller = typeof(GridTestController) }
				};
				res.Add(menu);
				return res;
			}
		}
	}
}
