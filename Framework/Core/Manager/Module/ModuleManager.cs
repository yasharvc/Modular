using Contracts.Module;

namespace Manager.Module
{
	public class ModuleManager : IManager
	{
		//protected Dictionary<string,Ass>
		public string GenerateNewToken() => new ModuleGUIDMaker().GetNew();
	}
}
