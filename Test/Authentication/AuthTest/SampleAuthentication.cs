using Contracts.Authentication;

namespace AuthTest
{
	public class SampleAuthentication : AuthenticationType, IAuthenticator
	{
		public SampleAuthentication() => Token = "AUTH-80506390-7941-4F2D-B1BC-4A003CAE4709";
		public override string GetDescription() => "یه دسترسی تستی";
		public string LoginPagePath => "/SampleAuth/Index";

		public bool IsAuthenticated() => false;
	}
}