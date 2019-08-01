using Contracts.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSThemeWithAuthentication.Components
{
	[ViewComponent(Name = "ModuleMenu")]
	public class ModulesMenuViewComponent : BaseViewComponent
	{
		public override async Task<IViewComponentResult> InvokeAsync()
		{
			var modules = Contracts.Hub.InvocationHub.GetModules();
			return await Task.FromResult(GetView("HomeArea", null));// new SideMenuHandler(modules, HttpContext)));
		}
	}
}
