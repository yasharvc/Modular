using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Meta
{
	public class ServiceFunction : Attribute
	{
		public string Description { get; set; } = "";
	}
	public class ServiceModel : Attribute
	{

	}
}
