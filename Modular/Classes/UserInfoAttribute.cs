using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Modular.Classes
{
	public class UserInfoAttribute : ResultFilterAttribute
	{
		AdminAuthentication perm = new AdminAuthentication();
		public UserInfoAttribute() { }

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			base.OnResultExecuting(context);
			perm.HttpContext = context.HttpContext;
			if (perm.IsAuthenticated())
			{
				((Controller)context.Controller).ViewData["User"] = perm.GetCurrentUser(context.HttpContext);
				perm.Extend();
			}
			else
			{
				if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor && controllerActionDescriptor.MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute)) != null)
					return;
				context.Cancel = true;
				context.HttpContext.Response.Redirect(perm.LoginPagePath);
			}
		}
	}
}
