using Contracts.Module;
using Contracts.Module.Menu;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Contracts.MVC
{
	public class SideMenuHandler
	{
		readonly HttpContext HttpContext;
		public Dictionary<MenuLegendName, Tuple<List<IMenu>, ModuleManifest>> MenuesByLegend { get; private set; } = new Dictionary<MenuLegendName, Tuple<List<IMenu>, ModuleManifest>>();

		public SideMenuHandler(IEnumerable<ModuleManifest> menus, HttpContext httpContext)
		{
			HttpContext = httpContext;
			getMenusByLegend(menus);
		}

		public string GetLegendTitle(MenuLegendName legendName)
		{
			switch (legendName)
			{
				case MenuLegendName.Defination:
					return "تعاریف";
				case MenuLegendName.Notices:
					return "اطلاع رسانی";
				case MenuLegendName.Operation:
					return "عملیات";
				case MenuLegendName.Reports:
					return "گزارشات";
			}
			return "";
		}

		public string GetLegendIcon(MenuLegendName legendName)
		{
			switch (legendName)
			{
				case MenuLegendName.Defination:
					return "icon-layers";
				case MenuLegendName.Notices:
					return "icon-bell";
				case MenuLegendName.Operation:
					return "icon-calculator";
				case MenuLegendName.Reports:
					return "icon-speech";
			}
			return "";
		}

		private void getMenusByLegend(IEnumerable<ModuleManifest> modules)
		{
			var allModules = modules;
			foreach (var module in allModules)
			{
				//module.HttpContext = HttpContext;
				foreach (var menu in module.Menus)
				{
					if (!MenuesByLegend.ContainsKey(menu.MenuLegend))
						MenuesByLegend[menu.MenuLegend] = new Tuple<List<IMenu>, ModuleManifest>(new List<IMenu>(), module);
					MenuesByLegend[menu.MenuLegend].Item1.Add(menu);
				}
			}
			foreach (var keyval in MenuesByLegend)
			{
				keyval.Value.Item1.Sort((IMenu a, IMenu b) =>
				{
					return a.Title.CompareTo(b.Title);
				});
			}
		}
	}
}
