using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Contracts.ViewComponent
{
	public interface IViewComponent
	{
		Task<IViewComponentResult> InvokeAsync();
	}
}