using Contracts;
using Contracts.Module;
using System.Reflection;

namespace BSThemeWithAuthentication
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest()
		{
			Name = "BTThemeWithAuth";
			Description = "تم ساده بوت استرپ همراه لایه امنیتی";
			Token = "MDL-56DFF4F9-91DE-4B70-A1B7-2A78EAD96363";
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}

		public string LayoutPathInsideModule => "/Views/Shared/_Layout.cshtml";
	}
}
