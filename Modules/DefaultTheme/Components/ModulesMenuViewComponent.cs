using Contracts.MVC;
using Contracts.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DefaultTheme.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			var modules = Contracts.Hub.InvocationHub.GetModules();
			return await Task.FromResult(GetView(nameof(DefaultTheme), new SideMenuHandler(modules,HttpContext)));
		}
	}
}