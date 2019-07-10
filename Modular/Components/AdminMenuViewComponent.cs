using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Modular.Components
{
	[ViewComponent(Name = "AdminMenu")]
	public class AdminMenuViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return await Task.FromResult((IViewComponentResult)View("AdminMenu"));
		}
	}
}