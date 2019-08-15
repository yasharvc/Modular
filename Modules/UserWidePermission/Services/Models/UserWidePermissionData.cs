using Contracts.Meta;

namespace UserWidePermission.Services.Models
{
	[ServiceModel]
	public class UserWidePermissionData
	{
		public int ID { get; set; }

		public int PermissionID { get; set; }

		public int UserID { get; set; }

		public string Value { get; set; }
	}
}
