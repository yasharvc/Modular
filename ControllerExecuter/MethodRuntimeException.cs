using System;
using System.Runtime.Serialization;

namespace ControllerExecuter
{
	[Serializable]
	internal class MethodRuntimeException : Exception
	{
		private Exception innerException;

		public MethodRuntimeException()
		{
		}

		public MethodRuntimeException(Exception innerException)
		{
			this.innerException = innerException;
		}

		public MethodRuntimeException(string message) : base(message)
		{
		}

		public MethodRuntimeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MethodRuntimeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}