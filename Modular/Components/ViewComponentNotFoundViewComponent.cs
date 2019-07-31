using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modular.Components
{
	[ViewComponent(Name = "ViewComponentNotFound")]
	public class ViewComponentNotFoundViewComponent : ViewComponent
	{
		string name = "";
		public ViewComponentNotFoundViewComponent(string viewComponentName) => name = viewComponentName;
		public async Task<IViewComponentResult> InvokeAsync() => await Task.FromResult((IViewComponentResult)Content($"{name} View Component Not Found"));
	}
}