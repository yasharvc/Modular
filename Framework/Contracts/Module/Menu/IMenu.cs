using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Module.Menu
{
	public interface IMenu
	{
		string Title { get; }
		string Icon { get; }
		Link Link { get; }
		MenuLegendName MenuLegend { get; }
		List<IMenu> SubMenus { get; }
	}
}
