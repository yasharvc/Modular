using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace DefaultTheme
{
	public class UserAuthenticator
	{
		private readonly string HomeAreaUserToken = "_HOME_USER_TOKEN_";

		private string GetToken(HttpContext context)
		{
			if (context == null || context.Request == null || context.Request.Cookies[HomeAreaUserToken] == null || !context.Request.Cookies.ContainsKey(HomeAreaUserToken)) return "";
			return context.Request.Cookies[HomeAreaUserToken] ?? "";
		}

		public bool Authenticate(HttpContext ctrl, string userName, string password)
		{
			var auth = new Authentication();
			var token = Authenticate(userName, password, UserType.SiteUser);
			if (!string.IsNullOrEmpty(token))
			{
				Authenticate(ctrl, token);
				return true;
			}
			return false;
		}

		private void Authenticate(HttpContext context, string token)
		{
			context.Response.Cookies.Append(HomeAreaUserToken, token);
		}

		private string Authenticate(string userName, string password, UserType siteUser)
		{
			var user = new User { UserName = userName, Password = password };
			return user.Authenticate(siteUser);
		}

		public void Disprove(HttpContext ctrl)
		{
			new User().LogOutByToken(GetToken(ctrl));
			ctrl.Response.Cookies.Delete(HomeAreaUserToken);
		}

		public bool IsAuthenticated(HttpContext ctrl)
		{
			if (ctrl != null)
			{
				return new User().IsTokenValid(GetToken(ctrl));
			}
			else
			{
				return false;
			}
		}
	}
}
