using Contracts.Authentication;
using Contracts.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserWidePermission.Services.Models;
using UserWidePermission.ViewModels;

namespace UserWidePermission.Controllers
{
	[AuthenticationType(typeof(AdminAuthentication))]
	public class ConfigController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return this.GetView(nameof(Index), new ConfigViewModel());
		}
		[HttpGet]
		public JsonResult GetUserPermissions(string moduleName, int userID)
		{
			return Json(new ConfigViewModel(false).GetUserPermission(moduleName, userID));
		}
		[HttpPost]
		public JsonResult SaveUserPermission(List<UserWidePermissionData> list)
		{
			if (new ConfigViewModel(false).SavePermissionData(list))
				return Json(new { result = true });
			else
				return Json(new { result = false });
		}
	}
}