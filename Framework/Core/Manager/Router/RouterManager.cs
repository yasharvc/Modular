using ControllerExecuter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Router
{
	public class RouterManager
	{
		protected ControllerExecuter.ControllerExecuter ControllerExecuter { get; set; } = new ControllerExecuter.ControllerExecuter();
	}
}
