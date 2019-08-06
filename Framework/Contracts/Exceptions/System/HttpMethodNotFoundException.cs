using System;

namespace Contracts.Exceptions.System
{
	public class HttpMethodNotFoundException : SystemException
	{
		public HttpMethodNotFoundException(string methodName) : base(methodName) { }
	}
}
