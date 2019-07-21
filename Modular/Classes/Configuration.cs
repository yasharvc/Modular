﻿using Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modular.Classes
{
	public class Configuration
	{
		public IHttpContextAccessor HttpContextAccessor { get; set; }
		public Configuration(IHttpContextAccessor httpContextAccessor) => HttpContextAccessor = httpContextAccessor;

		public string DataBaseConnectionString()
		{
			if (HttpContextAccessor.HttpContext.Items.ContainsKey(Consts.CONTEXT_ITEM_IS_IN_DEBUG))
				return "OK";
			else
				return "CANCEL";
		}
	}
}