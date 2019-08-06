using Contracts.Authentication;
using Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace AuthTest
{
	public class SampleAuthentication : Authentication
	{
		public SampleAuthentication() => Token = "AUTH-80506390-7941-4F2D-B1BC-4A003CAE4709";
		public override string GetDescription() => "یه دسترسی تستی";
		public override string LoginPagePath => "/SampleAuth/Index";

		public override bool IsAuthenticated() => false;

		public override User GetCurrentUser(HttpContext ctx)
		{
			throw new System.NotImplementedException();
		}
	}
}