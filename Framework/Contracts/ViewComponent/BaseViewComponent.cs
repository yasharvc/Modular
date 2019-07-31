using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;

namespace Contracts.ViewComponent
{
	public abstract class BaseViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent, IViewComponent
	{
		public new HttpContext HttpContext { get; set; }
		public new HttpRequest Request => HttpContext.Request;

		public BaseViewComponent()
		{
			HttpContext = base.HttpContext;
		}

		public IViewComponentResult GetView(string ModuleName, object model = null) => GetView(ModuleName, null, model);

		public IViewComponentResult GetView(string ModuleName, string viewName = null, object model = null)
		{
			var ViewComponentName = ((ViewComponentAttribute)GetType().GetCustomAttribute(typeof(ViewComponentAttribute))).Name;
			if (string.IsNullOrEmpty(viewName))
				viewName = ViewComponentName;
			return _GetView(ModuleName, ViewComponentName, viewName, model);
		}
		private IViewComponentResult _GetView(string ModuleName, string ViewComponentName, string ViewName, object model = null)
		{
			if (Hub.InvocationHub.IsModuleInDebugMode())
				return View(ViewComponentName, model);
			return View($"~/{Consts.MODULES_BASE_PATH}/{ModuleName}/Pages/Shared/Components/{ViewComponentName}/{ViewName}.cshtml", model);
		}

		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}