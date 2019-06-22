using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Exceptions.Application
{
	public class MethodNotAllowedException : ApplicationException
	{
		public string Controller { get; set; }
		public string Action { get; set; }
		public Enums.HttpMethod RequestedMethod { get; set; }

		public MethodNotAllowedException(string controller, string action, Enums.HttpMethod method) : base()
		{
			Controller = controller;
			Action = action;
			RequestedMethod = method;
		}
	}
}
