using System.Collections.Generic;

namespace Contracts.Module.Menu
{
	public class Menu : IMenu
	{
		public string Title { get; set; }
		public string Icon { get; set; }
		public Link Link { get; set; }
		public List<IMenu> SubMenus { get; set; } = new List<IMenu>();
		public MenuLegendName MenuLegend { get; set; } = MenuLegendName.Custom;
	}
}
