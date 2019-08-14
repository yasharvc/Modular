namespace Modular.Classes
{
	public class AdminUserAuthentication : Contracts.Authentication.CookieBasedUserAuthentication
	{
		public AdminUserAuthentication() : base("_MA_USER_TOKEN_", Contracts.Models.UserType.Administrator) { }
	}
}