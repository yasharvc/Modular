using Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Contracts.Authentication
{
	public abstract class CookieBasedUserAuthentication
	{
		private readonly int ExtendTimeInMinute = 10;
		private UserType UserType { get; set; } = UserType.SiteUser;

		private string HomeAreaUserToken { get; set; }

		public CookieBasedUserAuthentication() : this("_HOME_USER_TOKEN_", UserType.SiteUser) { }

		public CookieBasedUserAuthentication(string cookieName,UserType userType)
		{
			HomeAreaUserToken = cookieName;
			UserType = userType;
		}
		private string GetToken(HttpContext context)
		{
			if (context == null || context.Request == null || context.Request.Cookies[HomeAreaUserToken] == null || !context.Request.Cookies.ContainsKey(HomeAreaUserToken)) return "";
			return context.Request.Cookies[HomeAreaUserToken] ?? "";
		}

		public bool Authenticate(HttpContext ctrl, string userName, string password)
		{
			var token = Authenticate(userName, password);
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

		private string Authenticate(string userName, string password)
		{
			var user = new User { UserName = userName, Password = password };
			return user.Authenticate(UserType);
		}

		public void Disprove(HttpContext ctrl)
		{
			new User().LogOutByToken(GetToken(ctrl));
			ctrl.Response.Cookies.Delete(HomeAreaUserToken);
		}

		public bool IsAuthenticated(HttpContext ctrl)
		{
			Debug.WriteLine("IsAuthenticated");
			if (ctrl != null)
			{
				Debug.WriteLine("ctrl != null");
				Debug.WriteLine(GetToken(ctrl));
				return new User().IsTokenValid(GetToken(ctrl));
			}
			else
			{
				return false;
			}
		}

		public User GetUser(HttpContext context) => User.GetUserByToken(GetToken(context));

		public bool ExtendTokenTime(HttpContext ctx) => new User().ExtendTokenTime(GetToken(ctx), ExtendTimeInMinute);
	}
}
