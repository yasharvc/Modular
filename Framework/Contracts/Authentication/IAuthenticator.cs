namespace Contracts.Authentication
{
	public interface IAuthenticator
	{
		string LoginPagePath { get; }
		bool IsAuthenticated();
	}
}