using Contracts.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DefaultTheme.Components
{
	[ViewComponent(Name = "HomePageViewComponents")]
	public class HomePageViewComponentsViewComponent : BaseViewComponent
	{
		public async override Task<IViewComponentResult> InvokeAsync()
		{
			var viewComponents = Contracts.Hub.InvocationHub.GetModules().Select(m => 
			new KeyValuePair<string,Dictionary<string,BaseViewComponent>>( m.Name, m.HomePageViewComponents ));
			return await Task.FromResult(GetView(nameof(DefaultTheme), viewComponents));
		}
	}
}
