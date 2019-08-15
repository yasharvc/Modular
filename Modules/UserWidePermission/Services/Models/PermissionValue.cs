using Contracts.Meta;

namespace UserWidePermission.Services.Models
{
	[ServiceModel]
	public class PermissionValue
	{
		public string PermissionName { get; set; }
		public bool Value { get; set; }
	}
}
