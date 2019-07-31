namespace Contracts.Hub
{
	public class InvocationHub
	{
		public static object InvocationHubProvider = null;
		public static bool IsModuleInDebugMode(bool? desired = null)
		{
			if (desired.HasValue)
				return desired.Value;
			return InvocationHubProvider == null;
		}
	}
}