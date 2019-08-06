using Contracts;
using Microsoft.AspNetCore.Http;

namespace Modular.Classes
{
	public class Configuration
	{
		public IHttpContextAccessor HttpContextAccessor { get; set; }
		public Configuration(IHttpContextAccessor httpContextAccessor) => HttpContextAccessor = httpContextAccessor;

		public string DataBaseConnectionString()
		{
			if (HttpContextAccessor.HttpContext.Items.ContainsKey(Consts.CONTEXT_ITEM_IS_IN_DEBUG))
				return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB_DEBUG;User ID=sa;Password=@321@123#;";
			else
				return "Data Source=192.168.0.56;Initial Catalog=CMMS_WEB;User ID=sa;Password=@321@123#;";
		}
	}
}