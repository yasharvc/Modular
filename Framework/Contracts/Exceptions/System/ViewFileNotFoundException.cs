using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Exceptions.System
{
	public class ViewFileNotFoundException : SystemException
	{
		public string ModuleName { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }

		public ViewFileNotFoundException(string mdlName, string ctrlName, string actionName)
		{
			ModuleName = mdlName;
			ControllerName = ctrlName;
			ActionName = actionName;
		}
	}
}