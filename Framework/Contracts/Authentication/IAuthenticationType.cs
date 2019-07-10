namespace Contracts.Authentication
{
	public interface IAuthenticationType
	{
		string Token { get; }
		string GetCode();
		string GetDescription();
	}
}
