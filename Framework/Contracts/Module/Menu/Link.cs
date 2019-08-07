using Microsoft.AspNetCore.Mvc;
using System;

namespace Contracts.Module.Menu
{
	public class Link
	{
		private Type _controller;

		public Type Controller { get => _controller;
			set
			{
				var type = typeof(ControllerBase);
				if (value.IsAssignableFrom(type) || value.IsInstanceOfType(type) || value.IsSubclassOf(type))
					_controller = value;
				else
					throw new Exception("Is not controller type");
			}
		}
		public string Action { get; set; }
		public string ControllerName => Controller.Name.Replace("controller", "", StringComparison.OrdinalIgnoreCase);
	}
}