using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Modular.Components
{
	[ViewComponent(Name = "AdminMenu")]
	public class AdminMenuViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var modules = Startup.Manager.ModuleManager.GetModules().Select(m => m.Manifest);
			return await Task.FromResult((IViewComponentResult)View("AdminMenu", modules));
		}
	}
}