using Contracts.Meta;

namespace UserWidePermission.Services.Models
{
	[ServiceModel]
	public class PermissionSavingData
	{
		public string ModuleName { get; set; }
		public string PermissionName { get; set; }
		public int UserID { get; set; }
		public string Value { get; set; }
	}
}
