using System;

namespace Contracts
{
	public class RequestInformation
	{
		public string HeaderHost { get; set; }
		public string HeaderReferer { get; set; }
		public string UrlRequestPart { get; set; }
	}
}
