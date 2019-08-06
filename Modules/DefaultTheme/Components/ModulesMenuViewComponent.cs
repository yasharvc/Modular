using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Contracts.ViewComponent;
using Contracts.MVC;

namespace DefaultTheme.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			var modules = Contracts.Hub.InvocationHub.GetModules();
			return await Task.FromResult(GetView("HomeArea", new SideMenuHandler(modules,HttpContext)));
		}
	}
}
