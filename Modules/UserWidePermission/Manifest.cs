using Contracts.Module;
using Contracts.Module.Menu;
using System.Collections.Generic;
using UserWidePermission.Controllers;

namespace UserWidePermission
{
	public class Manifest : ModuleManifest
	{
		private IEnumerable<IMenu> adminPanel;
		public Manifest():base("UserWidePermission","دسترسی دهی داخل ماژول","MDL-28C32B7A-130B-4548-878D-913B07BD7291")
		{

		}

		public override IEnumerable<IMenu> AdminMenu
		{
			get
			{
				PrepareAdminPanel();
				return adminPanel;
			}
		}

		private void PrepareAdminPanel()
		{
			if (adminPanel == null)
			{
				adminPanel = new List<IMenu>();
				PrepareAdminMenus();
			}
		}

		private void PrepareAdminMenus()
		{
			var menus = adminPanel as List<IMenu>;
			var menu = new Menu
			{
				Title = "صفحه تنظیم",
				Icon = "fa fa-users",
				Link = new Link { Action = "Index", Controller = typeof(ConfigController) }
			};
			menus.Add(menu);
		}
	}
}
