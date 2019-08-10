using Contracts;
using Contracts.Module;
using Contracts.ViewComponent;
using System.Collections.Generic;

namespace TestWebApplication
{
	public class Manifest : ModuleManifest, IThemeProvider
	{
		public Manifest() : base("TestModule", "ماژول تستی", "MDL-7F3B064B-C033-4D5C-83C4-EDDED38B5A18") { }

		public string LayoutPathInsideModule => "/Views/Shared/_Layout.cshtml";

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents => new Dictionary<string, BaseViewComponent>();

		public override Dictionary<string, BaseViewComponent> ViewComponents => new Dictionary<string, BaseViewComponent>();

		public override BaseViewComponent GetCustomViewComponent(string name) => null;
	}
}