using Contracts.Meta;

namespace UserWidePermission.Services.Models
{
	[ServiceModel]
	public class UserWidePermissions
	{
		public int ID { get; set; }

		public string ModuleName { get; set; }

		public string PermissionName { get; set; }

		public string PermissionTitle { get; set; }
	}
}
