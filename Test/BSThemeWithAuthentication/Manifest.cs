using BSThemeWithAuthentication.Components;
using Contracts;
using Contracts.Module;
using Contracts.ViewComponent;
using System.Collections.Generic;
using System.Reflection;

namespace BSThemeWithAuthentication
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest()
		{
			Name = "BTThemeWithAuth";
			Description = "تم ساده بوت استرپ همراه لایه امنیتی";
			Token = "MDL-56DFF4F9-91DE-4B70-A1B7-2A78EAD96363";
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}

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
	}
}
