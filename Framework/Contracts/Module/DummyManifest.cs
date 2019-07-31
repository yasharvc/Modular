using System;
using System.Collections.Generic;
using System.Text;
using Contracts.ViewComponent;

namespace Contracts.Module
{
	public class DummyManifest : ModuleManifest
	{
		string name = "DummyName";
		string title = "Dummy Title";
		string desc = "Dummy Description";

		public override Dictionary<string, BaseViewComponent> HomePageViewComponents => throw new NotImplementedException();

		public override Dictionary<string, BaseViewComponent> ViewComponents => throw new NotImplementedException();

		public DummyManifest(string name) : this(name, "", "") { }
		public DummyManifest(string name, string title) : this(name, title, "") { }
		public DummyManifest(string name, string title, string description)
		{
			this.name = Name =name;
			this.title = title;
			this.desc = Description = description;
			Version = new Version(1, 1);
		}

		public override BaseViewComponent GetCustomViewComponent(string name) => null;
	}
}
