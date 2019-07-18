using Contracts.Hub;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Contracts.Razor
{
	public static class HTMLHelper
	{
		public static string GetThemeLayoutPath(this IHtmlHelper helper)
		{
			var context = helper.ViewContext.HttpContext;
			if (InvocationHub.IsModuleInDebugMode() || !context.Items.ContainsKey(Consts.CONTEXT_ITEM_KEY_THEME_LAYOUT_PATH))
				return "_Layout";
			return context.Items[Consts.CONTEXT_ITEM_KEY_THEME_LAYOUT_PATH].ToString();
		}
	}
}
