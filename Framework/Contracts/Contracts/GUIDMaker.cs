using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Contracts
{
	public abstract class GUIDMaker : IGUIDMaker
	{
		public string Prefix { get; set; } = "MDL";
		public GUIDMaker(string prefix) => Prefix = prefix.ToUpper();
		public string GetNew() => CreateCode(Guid.NewGuid());

		internal string GetAnonymouse() => CreateCode(new Guid());

		internal string CreateCode(Guid guid) => $"{Prefix}-{guid.ToString().ToUpper()}";
	}
}
