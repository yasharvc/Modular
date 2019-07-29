using System;

namespace Contracts.Exceptions.System
{
	public class MethodRuntimeException : SystemException
	{
		public string RealStackTrace { get; set; }
		public MethodRuntimeException(Exception mainException) : base(mainException.Message) => RealStackTrace = mainException.StackTrace;
		public MethodRuntimeException(string stackTrace) => RealStackTrace = stackTrace;
	}
}