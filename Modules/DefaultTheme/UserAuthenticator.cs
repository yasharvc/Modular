using Microsoft.AspNetCore.Http;

namespace DefaultTheme
{
	public class UserAuthenticator
	{
		private readonly string HomeAreaUserToken = "_HOME_USER_TOKEN_";

		public bool Authenticate(HttpContext ctrl, string userName, string password)
		{
			//var auth = new Authentication();
			//var token = auth.Authenticate(userName, password, UserType.SiteUser);
			//if (!string.IsNullOrEmpty(token))
			//{
			//	Authenticate(ctrl, token);
			//	return true;
			//}
			//return false;
			return true;
		}

		public void Disprove(HttpContext ctrl)
		{
			//new User().LogOutByToken(GetToken(ctrl));
			ctrl.Response.Cookies.Delete(HomeAreaUserToken);
		}

		public bool IsAuthenticated(HttpContext ctrl)
		{
			if (ctrl != null)
			{
				//return new Authentication().IsTokenValid(GetToken(ctrl));
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
