using Contracts.Meta;
using System;

namespace BSThemeWithAuthentication
{
	[ServiceModel]
	public class SimpleService : IService
	{
		public int? Age { get; set; }
		public string FullName { get; set; }
		public string Name => throw new NotImplementedException();
		public string Description => throw new NotImplementedException();
		[ServiceFunction]
		public string GetName(string name) => $"Your name is {name}!";
	}
}