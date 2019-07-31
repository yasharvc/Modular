using Contracts;
using Contracts.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Modular.Components
{
	[ViewComponent(Name = "Renderer")]
	public class RendererViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string moduleName, string viewComponentName)
		{
			var manifest = Startup.Manager.ModuleManager[moduleName].Manifest;
			BaseViewComponent cmp = null;
			if (manifest.HomePageViewComponents.ContainsKey(viewComponentName))
				cmp = manifest.HomePageViewComponents[viewComponentName];
			else if (manifest.ViewComponents.ContainsKey(viewComponentName))
				cmp = manifest.ViewComponents[viewComponentName];
			else
				cmp = manifest.GetCustomViewComponent(viewComponentName);
			if (cmp != null)
			{
				HttpContext.Items[Consts.CONTEXT_ITEM_KEY_THEME_MODULE_NAME] = moduleName;
				cmp.HttpContext = HttpContext;
				return await cmp.InvokeAsync();
			}
			else
			{
				return await new ViewComponentNotFoundViewComponent($"{moduleName}.{viewComponentName}").InvokeAsync();
			}
		}
	}
}
