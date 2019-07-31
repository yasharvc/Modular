using Contracts.Hub;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

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

		public static async Task<IHtmlContent> Component(this IViewComponentHelper viewComponentHelper, string moduleName, string viewComponentName)
		{
			if (InvocationHub.IsModuleInDebugMode())
				return await viewComponentHelper.InvokeAsync(viewComponentName);
			else
				return await viewComponentHelper.InvokeAsync("Renderer", new { moduleName, viewComponentName });
		}
	}
}
