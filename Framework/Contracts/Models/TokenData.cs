using Microsoft.AspNetCore.Mvc;
using System;

namespace Contracts.Security
{
	public class TokenData
	{
		public OkObjectResult Token { get; set; }
		public DateTime ExpireTime { get; set; } = new DateTime().Add(new TimeSpan(0, 115, 0));

	}
}
